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
        public int ObjectsCollected { get; set; }
        public int RequiredObjects { get; set; }

        private const double DEFAULT_TIME_LEFT = 5000;
        private const int DEFAULT_REQUIRED_OBJECTS = 2;

        public DayInfo()
        {
            RequiredObjects = DEFAULT_REQUIRED_OBJECTS;
            TimeLeft = DEFAULT_TIME_LEFT;
            InitialTime = TimeLeft;
            Number = 1;
            ObjectsCollected = 0;
        }
        public DayInfo(int n)
        {
            Number = n;
            TimeLeft = DEFAULT_TIME_LEFT;
            InitialTime = TimeLeft;
            RequiredObjects = 2 * n;
            ObjectsCollected = 0;
        }

        public override string ToString()
        {
            return $"Day {Number} - {TimeSpan.FromSeconds(TimeLeft):mm\\:ss}";
        }
    }
}
