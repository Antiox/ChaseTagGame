using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class OnTimeChangedEvent : IGameEvent
    {
        public float TimeLeft { get; set; }
        public float InitialTime { get; set; }

        public OnTimeChangedEvent(double initialTime, double timeLeft)
        {
            TimeLeft = (float)timeLeft;
            InitialTime = (float)initialTime;
        }
    }
}
