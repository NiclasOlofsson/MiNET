using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Commands
{
    public class RequirementCheckResult
    {
        public string Reason { get; }
        public bool Success { get; }

        public RequirementCheckResult(bool success) : this(success, success ? "Requirement is filled" : "One of the requirements is not filled")
        {

        }

        public RequirementCheckResult(bool success, string reason)
        {
            Success = success;
            Reason = reason;
        }
    }
}
