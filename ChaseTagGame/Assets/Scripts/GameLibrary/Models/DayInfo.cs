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
        public double InitialTime { get; set; }

        private const double DEFAULT_TIME_LEFT = 5;

        public DayInfo()
        {
            TimeLeft = DEFAULT_TIME_LEFT;
            InitialTime = TimeLeft;
            Number = 1;
        }
        public DayInfo(int n)
        {
            Number = n;
            TimeLeft = DEFAULT_TIME_LEFT;
            InitialTime = TimeLeft;
        }

        public override string ToString()
        {
            return $"Day {Number} - {TimeSpan.FromSeconds(TimeLeft):mm\\:ss}";
        }
    }
}
