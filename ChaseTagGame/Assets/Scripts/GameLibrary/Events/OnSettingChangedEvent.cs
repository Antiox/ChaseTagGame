using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class OnSettingChangedEvent : IGameEvent
    {
        public Settings Setting { get; set; }
        public object Value { get; set; }

        public OnSettingChangedEvent(Settings setting, object value)
        {
            Setting = setting;
            Value = value;
        }
    }

    public enum Settings
    {
        MouseSensitivity
    }
}
