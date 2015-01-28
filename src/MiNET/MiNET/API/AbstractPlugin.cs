using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.API
{
    public abstract class MiNETPlugin : IMiNETPlugin
    {

        public virtual string PluginName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string PluginDescription
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string PluginVersion
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string Author
        {
            get { throw new NotImplementedException(); }
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
