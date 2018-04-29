using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Net;

namespace MiNET.Scoreboards
{
    public class ScoreboardMetadataStore
    {

        private UUID uuid;

        private string objectiveName;

        private int score;

        public ScoreboardMetadataStore(UUID u, string o, int s)
        {
            uuid = u;
            objectiveName = o;
            score = s;
        }

        public UUID GetUuid()
        {
            return uuid;
        }

        public string GetObjective()
        {
            return objectiveName;
        }

        public int GetScore()
        {
            return score;
        }
    }
}
