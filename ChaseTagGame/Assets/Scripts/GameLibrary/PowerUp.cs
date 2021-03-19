using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public enum PowerUpType
    {
        Shield = 5,
    }

    public class PowerUp
    {
        public PowerUpType Type { get; set; }
        public float Duration { get; set; }


        public PowerUp(PowerUpType type)
        {
            Type = type;
            Duration = (int)type;
        }


        public void ExtendDuration(float duration)
        {
            Duration += duration;
        }

        public override string ToString()
        {
            return $"{Type} - {Duration}";
        }
    }
}
