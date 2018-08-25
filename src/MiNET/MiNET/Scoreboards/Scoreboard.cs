using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Net;

namespace MiNET.Scoreboards
{
    public class Scoreboard
    {

        public ScoreboardObjective objective { get; set; }
        public UUID id { get; set; }

        public Scoreboard()
        {
            id = new UUID(Guid.NewGuid().ToByteArray());
        }

        public ScoreboardObjective registerObjective(string name, ScoreboardCriteria criteria)
        {
            var obj = new ScoreboardObjective()
            {
                Name = name,
                Criteria = criteria
            };
            objective = obj;
            return objective;
        }


    }

}
