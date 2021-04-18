using UnityEngine;

namespace GameLibrary
{
    public class OnPlayerStayedInEnemyFovEvent : IGameEvent
    {
        public Collider Player { get; set; }
        public GameObject Enemy { get; set; }

        public OnPlayerStayedInEnemyFovEvent(Collider p, GameObject e)
        {
            Player = p;
            Enemy = e;
        }
    }
}
