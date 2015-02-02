using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.API
{
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
