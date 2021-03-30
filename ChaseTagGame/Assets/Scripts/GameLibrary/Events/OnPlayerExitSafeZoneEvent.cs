using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class OnPlayerExitSafeZoneEvent : IGameEvent
    {
        public GameObject Player { get; set; }

        public OnPlayerExitSafeZoneEvent(GameObject p)
        {
            Player = p;
        }
    }
}
