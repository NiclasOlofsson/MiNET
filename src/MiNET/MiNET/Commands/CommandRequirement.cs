﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Commands
{
    public interface CommandRequirement
    {
        RequirementCheckResult HasRequirement(CommandCall call);
    }
}
