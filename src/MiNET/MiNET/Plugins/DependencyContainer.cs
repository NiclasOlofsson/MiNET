using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using MiNET.Events;

namespace MiNET.Plugins
{
    /// <summary>
    ///     A dependency injection container
    /// </summary>
    public class DependencyContainer
    {
        private ConcurrentDictionary<Type, ServiceItem> Services { get; set; }

        public DependencyContainer()
        {
            Services = new ConcurrentDictionary<Type, ServiceItem>();
        }

        /// <summary>
        ///     Tries to resolve a service
        /// </summary>
        /// <param name="dependency">The resolved service.</param>
        /// <typeparam name="TType">The type of service to resolve</typeparam>
        /// <returns>Whether the service was able to be resolved or not</returns>
        public bool TryResolve<TType>(out TType dependency)
        {
            var resolved = Resolve<TType>();
            if (resolved.Equals(default(TType)))
            {
                dependency = default;
                return false;
            }

            dependency = (TType) resolved;

            return true;
        }

        /// <summary>
        ///     Tries to resolve a service
        /// </summary>
        /// <param name="dependency">The resolved service.</param>
        /// <param name="type">The type of service to resolve</param>
        /// <returns>Whether the service was able to be resolved or not</returns>
        public bool TryResolve(Type type, out object dependency)
        {
            var resolved = Resolve(type);
            if (resolved == null)
            {
                dependency = null;
                return false;
            }

            dependency = resolved;

            return true;
        }

        /// <summary>
        ///     Resolve a service
        /// </summary>
        /// <typeparam name="TType">The type to resolve.</typeparam>
        /// <returns>The resolved service</returns>
        public TType Resolve<TType>()
        {
            if (Services.TryGetValue(typeof(TType), out var value))
            {
                return (TType) value.GetInstance();
            }

            return default;
        }

        /// <summary>
        ///     Resolve a service
        /// </summary>
        /// <returns>The resolved service</returns>
        public object Resolve(Type type)
        {
            if (Services.TryGetValue(type, out var value))
            {
                return value.GetInstance();
            }

            return null;
        }

        /// <summary>
        ///     Remove a service from dependency injection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void Remove(Type type)
        {
            if (Services.TryRemove(type, out var serviceItem))
            {
                serviceItem.Dispose();
            }
        }
        
        /// <summary>
        ///     Remove a service from dependency injection
        /// </summary>
        public void Remove<TType>()
        {
            Remove(typeof(TType));
        }
        
        /// <summary>
        ///     Registers a new service
        /// </summary>
        /// <param name="lifetime">How long to keep the service alive for</param>
        /// <typeparam name="TType">The type of service to register</typeparam>
        /// <exception cref="DuplicateTypeException">Thrown when a service of the same type has already been registered</exception>
        public void Register<TType>(DependencyLifetime lifetime = DependencyLifetime.Singleton)
        {
            throw new NotImplementedException("Please use RegisterSingleton instead.");
            
            var type = typeof(TType);
            if (!Services.TryAdd(type, new ServiceItem(this, type, lifetime)))
            {
                throw new DuplicateTypeException();
            }
        }
        
        /// <summary>
        ///     Registers a new singleton service
        /// </summary>
        /// <param name="value">The instance to use for dependency injection</param>
        /// <typeparam name="TType">The type of service to register</typeparam>
        /// <exception cref="DuplicateTypeException">Thrown when a service of the same type has already been registered</exception>
        public void RegisterSingleton<TType>(TType value)
        {
            var type = typeof(TType);
            if (!Services.TryAdd(type, new ServiceItem(this, type, value)))
            {
                throw new DuplicateTypeException();
            }
        }

        /// <summary>
        ///     Registers a new singleton service
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void RegisterSingleton(Type type, object value)
        {
            if (!Services.TryAdd(type, new ServiceItem(this, type, value)))
            {
                throw new DuplicateTypeException();
            }
        }

        /// <summary>
        ///     Use the DependencyContainer to create an instance for any type with a public constructor.
        /// </summary>
        /// <param name="type">The type of the instance to create</param>
        /// <returns>An instance of <paramref name="type"/></returns>
        /// <exception cref="MissingMethodException">No public constructors were found</exception>
        /// <exception cref="Exception">Could not resolve all required parameters</exception>
        public object CreateInstanceOf(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            
            if (constructors.Length == 0)
                throw new MissingMethodException($"Could not find a public constructor");

            List<object> parameters = new List<object>();
            
            ConstructorInfo resultingConstructor = null;
            foreach (var constructor in constructors)
            {
                var requiredParams = constructor.GetParameters();
                
                foreach (var param in requiredParams)
                {
                    if (!TryResolve(param.ParameterType, out var obj))
                        break;
                    
                    parameters.Add(obj);
                }

                if (parameters.Count == requiredParams.Length)
                {
                    resultingConstructor = constructor;
                    break;
                }
                
                parameters.Clear();
            }
            
            if (resultingConstructor == null)
                throw new Exception("Could not find suitable constructor.");

            var instance = resultingConstructor.Invoke(parameters.ToArray());
            return instance;
        }
        
        /// <summary>
        ///     Use the DependencyContainer to create an instance for any type with a public constructor.
        /// </summary>
        /// <exception cref="MissingMethodException">No public constructors were found</exception>
        /// <exception cref="Exception">Could not resolve all required parameters</exception>
        public TType CreateInstanceOf<TType>()
        {
            return (TType) CreateInstanceOf(typeof(TType));
        }

        private class ServiceItem : IDisposable
        {
            private DependencyContainer Parent { get; }
            
            public Type Type { get; }
            public DependencyLifetime Lifetime { get; }

            private object _value = null;
            public ServiceItem(DependencyContainer parent, Type type, DependencyLifetime lifetime)
            {
                Parent = parent;
                Type = type;
                Lifetime = lifetime;
            }

            public ServiceItem(DependencyContainer parent, Type type, object value)
            {
                Parent = parent;
                Type = type;
                Lifetime = DependencyLifetime.Singleton;
                _value = value;
            }

            public void Initiate()
            {
                
            }

            public object GetInstance()
            {
                if (Lifetime == DependencyLifetime.Singleton)
                {
                    return _value;
                }

                return Construct();
            }

            private object Construct()
            {
                throw new NotImplementedException();
            }

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
            }
        }
    }

    /// <summary>
    ///     Used to determine a services lifetime
    /// </summary>
    public enum DependencyLifetime
    {
        /// <summary>
        ///     Keep 1 instance throughout the service lifetime
        /// </summary>
        Singleton,
        
        /// <summary>
        ///     Create a new instance everytime it is requested
        /// </summary>
        Transient
    }
}