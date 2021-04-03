using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class OnObjectiveItemTriggerEnterEvent : IGameEvent
    {
        public GameObject Entity { get; set; }
        public GameObject Objective { get; set; }

        public OnObjectiveItemTriggerEnterEvent(GameObject entity, GameObject objective)
        {
            Entity = entity;
            Objective = objective;
        }
    }
}
