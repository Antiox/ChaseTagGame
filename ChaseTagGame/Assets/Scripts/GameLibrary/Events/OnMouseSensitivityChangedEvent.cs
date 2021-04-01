using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    class OnMouseSensitivityChangedEvent : IGameEvent
    {
        public float Sensitivity { get; set; }

        public OnMouseSensitivityChangedEvent(float sensitivity)
        {
            Sensitivity = sensitivity;
        }
    }
}
