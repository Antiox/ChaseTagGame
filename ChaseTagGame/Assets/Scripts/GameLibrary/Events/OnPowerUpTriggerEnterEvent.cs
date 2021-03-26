using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class OnPowerUpTriggerEnterEvent : IGameEvent
    {
        public GameObject PowerUp { get; set; }
        public GameObject Entity { get; set; }


        public OnPowerUpTriggerEnterEvent(GameObject powerUp, GameObject entity)
        {
            PowerUp = powerUp;
            Entity = entity;
        }
    }
}
