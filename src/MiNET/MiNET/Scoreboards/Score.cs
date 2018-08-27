using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Entities;
using MiNET.Net;

namespace MiNET.Scoreboards
{
    public class Score
    {
        public long ScoreboardId { get; set; }
        public ScoreboardObjective Objective { get; set; }
        public Entity Entity { get; set; }
        public string FakePlayer { get; set; }
        public int ScoreId { get; set; }
        public long Id { get; set; }
        public bool IsFake { get; set; }

        public Score(ScoreboardObjective obj, string fakeplayer)
        {
            ScoreboardId = Scoreboard.RandomId();
            Objective = obj;
            FakePlayer = fakeplayer;
            IsFake = true;
        }

        public Score(ScoreboardObjective obj, Entity entity)
        {
            ScoreboardId = Scoreboard.RandomId();
            Objective = obj;
            Entity = entity;
            Id = entity.EntityId;
            IsFake = false;
        }

        public void SetScore(int score)
        {
            ScoreId = score;
        }

        public int GetScore()
        {
            return ScoreId;
        }

    }
}
