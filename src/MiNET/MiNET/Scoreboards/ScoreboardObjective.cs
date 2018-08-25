using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Scoreboards
{
    public class ScoreboardObjective
    {

        public String Name { get; set; }
        public ScoreboardDisplaySlot DisplaySlot { get; set; }
        public ScoreboardCriteria Criteria { get; set; }
        public String DisplayName { get; set; }
        public List<Score> Scores = new List<Score>();

        public byte Sort = 1;

        public Score GetScore(String fakeplayer)
        {
            var score = new Score()
            {
                FakePlayer = fakeplayer
            };
            if (!Scores.Contains(score))
            {
                Scores.Add(score);
            } else
            {
                foreach(var scores in Scores)
                {
                    if (scores.FakePlayer.Equals(fakeplayer))
                    {
                        score = scores;
                    }
                }
            }
            return score;
        }

        public string CriteriaToString()
        {
            string s = "";
            switch (Criteria)
            {
                case ScoreboardCriteria.Dummy:
                    s = "dummy";
                    break;
            }
            return s;
        }

        public string SlotToString()
        {
            string slot = "";
            switch (DisplaySlot)
            {
                case ScoreboardDisplaySlot.Sidebar:
                    slot = "sidebar";
                    break;
                case ScoreboardDisplaySlot.List:
                    slot = "list";
                    break;
                case ScoreboardDisplaySlot.BellowName:
                    slot = "bellowname";
                    break;
            }
            return slot;
        }

    }
}
