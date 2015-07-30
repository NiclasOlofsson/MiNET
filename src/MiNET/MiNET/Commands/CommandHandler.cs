using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Commands
{
    public abstract class CommandHandler
    {
        public abstract void OnTrigger(CommandCall call);

        public virtual List<CommandRequirement> GetRequirements()
        {
            return new List<CommandRequirement>();
        }

        public RequirementCheckResult HasRequirements(CommandCall call)
        {
            foreach(CommandRequirement req in GetRequirements())
            {
                RequirementCheckResult result = req.HasRequirement(call);
                if(! result.Success)
                    return result;
            }
            return new RequirementCheckResult(true);
        }
    }
}
