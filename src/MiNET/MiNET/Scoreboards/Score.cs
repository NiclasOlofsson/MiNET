using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Net;

namespace MiNET.Scoreboards
{
    public class Score
    {
        public string FakePlayer { get; set; }
        public short ScoreId { get; set; }
        public UUID ScoreboardId { get; set; }

        public Score()
        {

        }

        public void SetScore(short score)
        {
            ScoreId = score;
        }

    }
}
