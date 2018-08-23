using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Scoreboards
{
    public class Scoreboard
    {

        public ScoreboardObjective objective;

        public Scoreboard()
        {
        }

        public void registerObjective(ScoreboardObjective objective)
        {
            this.objective = objective;
        }

    }

}
