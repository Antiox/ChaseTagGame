using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class OnGemTriggerEnterEvent : IGameEvent
    {
        public GameObject Entity { get; set; }
        public GameObject Gem { get; set; }

        public OnGemTriggerEnterEvent(GameObject entity, GameObject gem)
        {
            Entity = entity;
            Gem = gem;
        }
    }
}
