using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class DayInfo
    {
        public int Number { get; set; }
        public double TimeLeft { get; set; }

        private const double DEFAULT_TIME_LEFT = 240;

        public DayInfo()
        {
            TimeLeft = DEFAULT_TIME_LEFT;
            Number = 1;
        }
        public DayInfo(int n)
        {
            Number = n;
            TimeLeft = DEFAULT_TIME_LEFT;
        }

        public override string ToString()
        {
            return $"Day {Number} - {TimeSpan.FromSeconds(TimeLeft):mm\\:ss}";
        }
    }
}
