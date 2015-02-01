using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute
    {
        public string PluginName;
        public string Description;
        public string PluginVersion;
        public string Author;
    }

    public abstract class MiNETPlugin : IMiNETPlugin
    {
	    public MiNETPlugin()
	    {
		    
	    }

        public virtual void OnEnable()
        {
            throw new NotImplementedException();
        }

        public virtual void OnDisable()
        {
            throw new NotImplementedException();
        }
    }
}
