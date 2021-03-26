using UnityEngine;

namespace GameLibrary
{
    public class OnEnemyTriggerEnterEvent : IGameEvent
    {
        public GameObject Enemy { get; set; }
        public GameObject Entity { get; set; }


        public OnEnemyTriggerEnterEvent(GameObject enemy, GameObject entity)
        {
            Enemy = enemy;
            Entity = entity;
        }
    }
}
