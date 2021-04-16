using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public interface IEnemy
    {
        public GameObject GameObject { get; set; }
        public float DetectionAngle { get; set; }
        public float DetectionRange { get; set; }
        public EnemyBehavior Behavior { get; set; }
        public Vector3 LastPlayerKnownPosition { get; set; }
        public Vector3 LastPlayerKnownDirection { get; set; }
        public List<Vector3> Path { get; set; }

        public Vector3 GetStartingPositon();
        public void Start();
    }

    public enum EnemyBehavior
    {
        Scouting,
        Searching,
        Chasing
    }
}