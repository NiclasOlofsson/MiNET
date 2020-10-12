using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;

namespace MiNET.Events
{
	/// <summary>
	/// 	The <see cref="EventDispatcher"/> is responsible for dispatching and invoking all the registered <see cref="IEventHandler"/> methods
	/// </summary>
	public class EventDispatcher
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(EventDispatcher));

		private static readonly ThreadSafeList<Type> EventTypes = new ThreadSafeList<Type>(AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(GetEventTypes)
			.Select(p =>
			{
				Log.Info($"Registered event type \"{p.Name}\"");
				return p;
			}).ToArray());

		/// <summary>
		/// 	Registers a new <see cref="Event"/> type with the current EventDispatcher
		/// </summary>
		/// <typeparam name="TEvent">The type of the <see cref="Event"/> to register</typeparam>
		/// <exception cref="DuplicateTypeException">Thrown when the type of <typeparamref name="TEvent"/> has already been registered.</exception>
		public void RegisterEventType<TEvent>() where TEvent : Event
		{
			Type t = typeof(TEvent);
			if (!RegisterEventType(t))
			{
				throw new DuplicateTypeException();
			}
		}

		/// <summary>
		/// Registers a new <see cref="Event"/>
		/// </summary>
		/// <param name="type">The type of the <see cref="Event"/> to register</param>
		/// <returns>Whether the event was succesfully registered.</returns>
		public bool RegisterEventType(Type type)
		{
			if (RegisteredEvents.ContainsKey(type) || !EventTypes.TryAdd(type))
			{
				return false;
			}
			else
			{
				RegisteredEvents.Add(type, new EventDispatcherValues());
				Log.Info($"Registered event type \"{type.Name}\"");

				return true;
			}
		}
		
		/// <summary>
		/// 	Loads all types implementing the <see cref="Event"/> class
		/// </summary>
		/// <param name="assembly">The assembly containing the <see cref="Event"/> implementations</param>
		public void LoadFrom(Assembly assembly)
		{
			var count = GetEventTypes(assembly).Count(RegisterEventType);
			Log.Info($"Registered {count} event types from assembly {assembly.ToString()}");
		}

		/// <summary>
		/// 	Unloads all <see cref="Event"/>'s that were registered by specified assembly
		/// </summary>
		/// <param name="assembly">The assembly containing the types to be unloaded.</param>
		public void Unload(Assembly assembly)
		{
			int count = 0;
			foreach (var eventType in (from eventType in EventTypes
				where eventType.Assembly == assembly
				select eventType))
			{
				if (EventTypes.Remove(eventType))
					count++;

				RegisteredEvents.Remove(eventType);
			}
			
			Log.Info($"Unloaded {count} event types from assembly {assembly.ToString()}");
		}
		
		private static IEnumerable<Type> GetEventTypes(Assembly assembly)
		{
			return assembly.GetTypes().Where(p =>
			{
				if (p.IsClass && !p.IsAbstract && typeof(Event).IsAssignableFrom(p))
				{
					return true;
				}

				return false;
			});
		}

		private Dictionary<Type, EventDispatcherValues> RegisteredEvents { get; }
		protected MiNetServer Api { get; }
		private EventDispatcher[] ExtraDispatchers { get; }
		public EventDispatcher(MiNetServer openApi, params EventDispatcher[] dispatchers)
		{
			Api = openApi;
			ExtraDispatchers = dispatchers.Where(x => x != this).ToArray();

			RegisteredEvents = new Dictionary<Type, EventDispatcherValues>();
			foreach (var eventType in EventTypes)
			{
				RegisteredEvents.Add(eventType, new EventDispatcherValues());
			}
			//Log.Info($"Registered {RegisteredEvents.Count} event types!");
		}

		/// <summary>
		/// 	Registers all EventHandler methods with the current EventDispatcher.
		/// </summary>
		/// <param name="obj">The class to scan for EventHandlers</param>
		public void RegisterEvents(IEventHandler obj)
		{
			int count = 0;

			var type = typeof(Event);
			Type objType = obj.GetType();
			foreach (var method in objType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				EventHandlerAttribute attribute = method.GetCustomAttribute<EventHandlerAttribute>(false);
				if (attribute == null) continue;

				var parameters = method.GetParameters();
				if (parameters.Length != 1 || !type.IsAssignableFrom(parameters[0].ParameterType)) continue;

				var paramType = parameters[0].ParameterType;

			    EventDispatcherValues e = null;
                if (!RegisteredEvents.TryGetValue(paramType, out e))
			    {
			        if (EventTypes.TryAdd(paramType))
			        {
			            e = new EventDispatcherValues();
                        RegisteredEvents.Add(paramType, e);
			        }
			    }

				if (!e.RegisterEventHandler(attribute, obj, method))
				{
					Log.Warn($"Duplicate found for class \"{obj.GetType()}\" of type \"{paramType}\"");
				}
				else
				{
					count++;
				}
			}

			Log.Info($"Registered {count} event handlers for \"{obj}\"");
		}

		/// <summary>
		/// 	Unregisters all <see cref="EventHandlerAttribute"/> from the specified <see cref="IEventHandler"/> implementation from the current EventDispatcher
		/// 	After UnRegistering, the class will no longer get invoked when an event gets dispatched.
		/// </summary>
		/// <param name="obj">The implementation to unregister the eventhandlers for</param>
		public void UnregisterEvents(IEventHandler obj)
		{
			foreach (var kv in RegisteredEvents.ToArray())
			{
				kv.Value.Clear(obj);
			}
		}

		private async Task DispatchPrivate(Event e)
		{
			try
			{
				Type type = e.GetType();
				if (RegisteredEvents.TryGetValue(type, out EventDispatcherValues v))
				{
					await v.DispatchAsync(e);
				}
				else
				{
					Log.Warn($"Unknown event type found! \"{type}\"");
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error while dispatching event!", ex);
			}
		}

		/// <summary>
		/// 	Dispatches a new <see cref="Event"/> to all methods registered with an <see cref="EventHandlerAttribute"/>
		/// </summary>
		/// <param name="e">The event to dispatch</param>
		public void DispatchEvent(Event e)
		{
			DispatchPrivate(e).Wait();

			if (!e.IsCancelled)
			{
				foreach (var i in ExtraDispatchers)
				{
					i.DispatchPrivate(e).Wait();
					if (e.IsCancelled) break;
				}
			}

			if (Api.ConnectionInfo != null)
			{
				Interlocked.Increment(ref Api.ConnectionInfo.EventsDispatchedPerSecond);
			}
		}

		/// <summary>
		/// 	Dispatches 
		/// </summary>
		/// <param name="e"></param>
		/// <typeparam name="TEvent"></typeparam>
		/// <returns></returns>
		public async Task<TEvent> DispatchEventAsync<TEvent>(TEvent e) where TEvent : Event
		{
			await DispatchPrivate(e);

			List<Task> dispatchTasks = new List<Task>();
			
			if (!e.IsCancelled)
			{
				foreach (var i in ExtraDispatchers)
				{
					dispatchTasks.Add(i.DispatchPrivate(e));
					//if (e.IsCancelled) break;
				}
			}

			await Task.WhenAll(dispatchTasks);

			if (Api.ConnectionInfo != null)
			{
				Interlocked.Increment(ref Api.ConnectionInfo.EventsDispatchedPerSecond);
			}

			return e;
		}

		private class EventDispatcherValues
		{
		//	private ConcurrentDictionary<IEventHandler, MethodInfo> EventHandlers { get; }
            private Dictionary<EventPriority, List<Item>> Items { get; }
			//private SortedSet<Item> Items { get; set; }
			public EventDispatcherValues()
			{
                Items = new Dictionary<EventPriority, List<Item>>();
			    foreach (var prio in Enum.GetValues(typeof(EventPriority)))
			    {
                    Items.Add((EventPriority) prio, new System.Collections.Generic.List<Item>());
			    }
				//Items = new SortedSet<Item>();
			//	EventHandlers = new ConcurrentDictionary<IEventHandler, MethodInfo>();
			}

			public bool RegisterEventHandler(EventHandlerAttribute attribute, IEventHandler parent, MethodInfo method)
			{
			    Items[attribute.Priority].Add(new Item(attribute, parent, method));
			    return true;
				//return Items.Add(new Item(attribute, parent, method));
				/*if (!EventHandlers.TryAdd(parent, method))
				{
					return true;
				}
				return false;*/
			}

			public void Clear(IEventHandler parent)
			{
			    foreach (var priorityList in Items.ToArray())
			    {
			        try
			        {
			            var copy = priorityList.Value.ToArray();
			            foreach (var item in copy)
			            {
			                try
			                {
			                    if (item.Parent == parent)
			                    {
			                        if (priorityList.Value.Count > 0)
			                        {
			                            priorityList.Value.Remove(item);
			                        }
			                    }
			                }
			                catch (Exception x)
			                {

			                }
			            }
			        }
			        catch (Exception ex)
			        {

			        }
			    }
				//Items.RemoveWhere(x => x.Parent == parent);
				//MethodInfo method;
				//EventHandlers.TryRemove(parent, out method);
			}

			/*public void Dispatch(Event e)
			{
				object[] args = {
					e
				};

			    foreach (var priority in Items)
			    {
			        Parallel.ForEach(priority.Value.ToArray(), pair =>
			        {
			            if (e.IsCancelled &&
			                pair.Attribute.IgnoreCanceled)
			                return;

			            pair.Method.Invoke(pair.Parent, args);
			        });
                }
			}*/

			public async Task DispatchAsync(Event e)
			{
				object[] args = {
					e
				};

				foreach (var priority in Items)
				{
					Task[] tasks = new Task[priority.Value.Count];
					for (var index = 0; index < priority.Value.Count; index++)
					{
						var p = priority.Value[index];
						
						var method = p.Method;
						if (method.ReturnType == typeof(void))
						{
							tasks[index] = Task.Run(() =>
							{
								if (e.IsCancelled &&
								    p.Attribute.IgnoreCanceled)
									return;
								
								method.Invoke(p.Parent, args);
							});
						}
						else if (typeof(Task).IsAssignableFrom(method.ReturnType))
						{
							tasks[index] = Task.Run(async () =>
							{
								if (e.IsCancelled &&
								    p.Attribute.IgnoreCanceled)
									return;
								
								await (Task) method.Invoke(p.Parent, args);
							});
						}
					}

					await Task.WhenAll(tasks);
				}
			}

			private struct Item : IComparable<Item>
			{
				//public EventPriority Priority;
				public EventHandlerAttribute Attribute;
				public IEventHandler Parent;
				public MethodInfo Method;
				public Item(EventHandlerAttribute attribute, IEventHandler parent, MethodInfo method)
				{
					Attribute = attribute;
					Parent = parent;
					Method = method;
				}

				public int CompareTo(Item other)
				{
					int result = Attribute.Priority.CompareTo(other.Attribute.Priority);

					if (result == 0)
						result = Parent.GetHashCode().CompareTo(other.Parent.GetHashCode());
					
						return result;
				}
			}

			//private class ItemCompare
		}
	}
}
