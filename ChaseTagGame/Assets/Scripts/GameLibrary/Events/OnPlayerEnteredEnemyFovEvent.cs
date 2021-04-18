using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class OnPlayerEnteredEnemyFovEvent : IGameEvent
    {
        public Collider Player { get; set; }
        public GameObject Enemy { get; set; }

        public OnPlayerEnteredEnemyFovEvent(Collider p, GameObject e)
        {
            Player = p;
            Enemy = e;
        }
    }
}
