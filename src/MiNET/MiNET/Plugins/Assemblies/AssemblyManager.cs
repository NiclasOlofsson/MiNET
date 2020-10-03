using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using log4net;

namespace MiNET.Plugins.Assemblies
{
    internal class AssemblyManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssemblyManager));
        
        private ConcurrentDictionary<string, Assembly> AssemblyReferences { get; }
        
        public AssemblyManager()
        {
            AssemblyReferences = new ConcurrentDictionary<string, Assembly>();
            
            foreach (var referencedAssemblies in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!AssemblyReferences.ContainsKey(referencedAssemblies.GetName().Name))
                {
                    AssemblyReferences.TryAdd(referencedAssemblies.GetName().Name, referencedAssemblies);
					
                    foreach (var referenced in referencedAssemblies.GetReferencedAssemblies())
                    {
                        if (!AssemblyReferences.ContainsKey(referenced.Name))
                        {
                            var newlyLoaded = Assembly.Load(referenced);
                            AssemblyReferences.TryAdd(referenced.Name, newlyLoaded);
                        }
                    }
                }
            }
        }

        public bool TryLoadAssemblyFromFile(string assemblyName, string file, out Assembly assembly)
        {
            try
            {
                assembly = Assembly.LoadFrom(file);
                if (AssemblyReferences.TryAdd(assemblyName, assembly))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to load assembly {assemblyName} even tho its path was found!", ex);

                //assemblies = default(Assembly[])
            }

            assembly = null;
            return false;
        }

        public bool TryGetAssembly(string assemblyName, out Assembly assembly)
        {
            return AssemblyReferences.TryGetValue(assemblyName, out assembly);
        }
        
        public bool IsLoaded(string assemblyName, out Assembly outAssembly)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();//.Concat(AssemblyReferences.Values.ToArray()).ToArray();

            Assembly ooutAssembly =
                loadedAssemblies.FirstOrDefault(x => x != null && x.GetName().Name
                    .Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (ooutAssembly != null)
            {
                outAssembly = ooutAssembly;

                if (!AssemblyReferences.ContainsKey(assemblyName))
                {
                    AssemblyReferences.TryAdd(assemblyName, outAssembly);
                }
                
                return true;
            }
            
            return AssemblyReferences.TryGetValue(assemblyName, out outAssembly);
        }
    }
}