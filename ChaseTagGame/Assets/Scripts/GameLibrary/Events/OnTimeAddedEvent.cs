using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class OnTimeAddedEvent : IGameEvent
    {
        public double Amount { get; set; }

        public OnTimeAddedEvent(double a)
        {
            Amount = a;
        }
    }
}
