using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Scoreboards
{

    public enum DisplayTypes
    {
        DUMMY
    }

    public class ScoreboardDisplay
    {

        public DisplayTypes display;

        public ScoreboardDisplay() { }

        public void SetDisplayName(DisplayTypes type)
        {
            display = type;
        }

    }
}
