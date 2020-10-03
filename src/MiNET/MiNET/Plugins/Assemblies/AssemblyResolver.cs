using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using log4net;
using Mono.Cecil;

namespace MiNET.Plugins.Assemblies
{
    internal class AssemblyResolver
    {
	    private static readonly ILog Log = LogManager.GetLogger(typeof(AssemblyResolver));
	    
	    private AssemblyManager AssemblyManager { get; }
        public AssemblyResolver(AssemblyManager assemblyManager)
        {
	        AssemblyManager = assemblyManager;
	        
	        AppDomain.CurrentDomain.AssemblyResolve += PluginManagerOnAssemblyResolve;
        }
        
        private Assembly PluginManagerOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
	        try
	        {
		        //  AssemblyName name = new AssemblyName(args.Name);
		        AssemblyNameReference name = AssemblyNameReference.Parse(args.Name);
		        if (name.Name.EndsWith(".resources") && !name.Culture.EndsWith("neutral")) return null;
				
		        if (AssemblyManager.IsLoaded(name.Name, out Assembly loadedPluginAssembly))
		        {
			        return loadedPluginAssembly;
		        }

		        string rootPath = "";
		        if (args.RequestingAssembly != null && !string.IsNullOrWhiteSpace(args.RequestingAssembly.Location))
		        {
			        rootPath = Path.GetDirectoryName(args.RequestingAssembly.Location);
		        }

		        Assembly result = null;
		        if (TryFindAssemblyPath(name, rootPath, out string resultPath))
		        {
			        AssemblyManager.TryLoadAssemblyFromFile(name.Name, resultPath, out result);
			        //result = Assembly.LoadFile(resultPath);
		        }
		        else
		        {
			        var assembly = AppDomain.CurrentDomain.GetAssemblies()
				        .FirstOrDefault(x => x.GetName().Name == args.Name);
			        
			        if (assembly != null) 
				        result = assembly;
		        }

		        return result;
	        }
	        catch (Exception ex)
	        {
		        Log.Error($"Failed to resolve!", ex);
		        return null;
	        }
        }
        
        internal bool TryResolve(string path, ModuleDefinition module, out Assembly[] assemblies)
        {
	        //var proxy = Proxy.CreateProxy(domain);
	        List<AssemblyNameReference> assemblyNames = module.AssemblyReferences.ToList();

	        Dictionary<AssemblyNameReference, Assembly> resolvedAssemblies = new Dictionary<AssemblyNameReference, Assembly>();
	        Dictionary<AssemblyNameReference, string> resolvedPaths = new Dictionary<AssemblyNameReference, string>();
	        foreach (var assemblyName in assemblyNames)
	        {
		        if (AssemblyManager.IsLoaded(assemblyName.Name, out Assembly loadedAssembly))
		        {
			        resolvedAssemblies.Add(assemblyName, loadedAssembly);
			        continue;
		        }

		        try
		        {
			        if (TryFindAssemblyPath(assemblyName, path, out string resultPath))
			        {
				        resolvedPaths.Add(assemblyName, resultPath);
			        }
			        else
			        {
				        Log.Warn($"Plugin \"{module.FileName}\" requires \"{assemblyName}\" but it could not be found.");
				        //  assemblies = default(Assembly[]);
				        // return false;
			        }
		        }
		        catch(Exception e)
		        {
			        Log.Warn($"Could not find path for {assemblyName} - {e.ToString()}");
			        assemblies = default(Assembly[]);
			        return false;
		        }
	        }

	        foreach (var resolved in resolvedPaths)
	        {
		        if (AssemblyManager.TryLoadAssemblyFromFile(resolved.Key.Name, resolved.Value, out Assembly loaded))
		        {
			        resolvedAssemblies.TryAdd(resolved.Key, loaded);
		        }
	        }

	        assemblies = resolvedAssemblies.Values.ToArray();
	        return true;
        }

        private bool TryFindAssemblyPath(AssemblyNameReference name, string rootPath, out string resultingPath)
	    {
		    AssemblyDependencyResolver resolver = new AssemblyDependencyResolver(rootPath);
		    var p = resolver.ResolveAssemblyToPath(new AssemblyName(name.ToString()));
		    if (p != null)
		    {
			    resultingPath = p;
			    return true;
		    }
			    
		    string dllName = name.Name + ".dll";

		    var assemblyLocation = rootPath;

			string file = Path.Combine(assemblyLocation, dllName);

			string result = null;
			if (CompareFileToAssemblyName(file, name) == FileAssemblyComparisonResult.Match)
			{
				result = file;
			}
			else
			{
				foreach (var path in AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).Select(x => x.Location))
				{
					file = Path.Combine(path, dllName);
					if (CompareFileToAssemblyName(file, name) == FileAssemblyComparisonResult.Match)
					{
						result = file;

						break;
					}
				}
				/*string lastPath = _lastPath;
				if (!string.IsNullOrEmpty(lastPath))
				{
					file = Path.Combine(lastPath, dllName);
					if (CompareFileToAssemblyName(file, name) == FileAssemblyComparisonResult.Match)
					{
						result = file;
					}
				}
*/
				string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				string callingAssembliesPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

				if (result == null && executingPath != null)
				{
					file = Path.Combine(executingPath, dllName);
					if (File.Exists(Path.Combine(executingPath, dllName)))
					{
						result = file;
					}
				}

				if (result == null && callingAssembliesPath != null)
				{
					file = Path.Combine(callingAssembliesPath, dllName);
					if (CompareFileToAssemblyName(file, name) == FileAssemblyComparisonResult.Match)
					{
						result = file;
					}
				}
			}

		    if (string.IsNullOrWhiteSpace(result))
		    {
			    resultingPath = default(string);
			    return false;
			}
		    else
		    {
			    resultingPath = result;
			    return true;
		    }
		}

	    private enum FileAssemblyComparisonResult
	    {
			FileNotFound,
			NotEqual,
			Match
	    }

	    private FileAssemblyComparisonResult CompareFileToAssemblyName(string file, AssemblyNameReference name)
	    {
		    if (!File.Exists(file))
			    return FileAssemblyComparisonResult.FileNotFound;

		    var module = ModuleDefinition.ReadModule(file);
			//AssemblyName fileAssemblyName = AssemblyName.GetAssemblyName(file);
				// if (AssemblyName.ReferenceMatchesDefinition(fileAssemblyName, new AssemblyName(name.FullName)))
			if (name.Name.Equals(module.Assembly.Name.Name, StringComparison.InvariantCultureIgnoreCase))
		    {
			    return FileAssemblyComparisonResult.Match;
			}
		    else
		    {
			    return FileAssemblyComparisonResult.NotEqual;
		    }
	    }
    }
}