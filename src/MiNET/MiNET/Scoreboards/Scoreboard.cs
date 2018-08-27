using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MiNET.Net;

namespace MiNET.Scoreboards
{
    public class Scoreboard
    {

        public ScoreboardObjective objective { get; set; }
        public long id { get; set; }
        public bool AlreadyCreated = false;

        public Scoreboard()
        {
            id = -RandomId();
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

        public static long RandomId()
        {
            Random rnd = new Random();

            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long intRand = BitConverter.ToInt64(buf, 0);

            long result = (Math.Abs(intRand % (20000000000 - 10000000000)) + 10000000000);

            long random_seed = (long)rnd.Next(1000, 5000);
            random_seed = random_seed * result + rnd.Next(1000, 5000);
            long randomlong = ((long)(random_seed / 655) % 10000000001);
            return randomlong;
        }


    }

}
