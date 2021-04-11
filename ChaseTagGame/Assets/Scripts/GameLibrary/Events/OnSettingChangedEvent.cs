using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class OnSettingChangedEvent : IGameEvent
    {
        public SettingType Setting { get; set; }
        public dynamic Value { get; set; }

        public OnSettingChangedEvent(SettingType setting, dynamic value)
        {
            Setting = setting;
            Value = value;
        }
    }
}
