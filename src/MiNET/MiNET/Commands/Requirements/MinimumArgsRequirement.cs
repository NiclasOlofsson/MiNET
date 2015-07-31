using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Commands.Requirements
{
    public class MinimumArgsRequirement : CommandRequirement
    {
        public int Count { get; set; }

        public MinimumArgsRequirement(int count)
        {
            Count = count;
        }

        public RequirementCheckResult HasRequirement(CommandCall call)
        {
            bool enough = call.Args.Count >= Count;
            string reason = enough ? "Enough arguments" : "Not enough arguments";
            return new RequirementCheckResult(enough, reason);
        }
    }
}
