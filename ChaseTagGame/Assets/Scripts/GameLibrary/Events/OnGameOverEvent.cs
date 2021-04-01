using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class OnGameOverEvent : IGameEvent
    {
        public DayInfo Day { get; set; }

        public OnGameOverEvent(DayInfo day)
        {
            Day = day;
        }
    }
}
