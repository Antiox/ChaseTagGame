using UnityEngine;

namespace GameLibrary
{
    public class OnPlayerLeftEnemyFovEvent : IGameEvent
    {
        public Collider Player { get; set; }
        public GameObject Enemy { get; set; }

        public OnPlayerLeftEnemyFovEvent(Collider p, GameObject e)
        {
            Player = p;
            Enemy = e;
        }
    }
}
