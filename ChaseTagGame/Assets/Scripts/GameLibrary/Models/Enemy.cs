using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class Enemy : IEnemy
    {
        public static List<IEnemy> Instances { get; set; }


        public GameObject GameObject { get; set; }
        public float DetectionAngle
        {
            get
            {
                return detectionAreaScript.Angle;
            }
            set
            {
                detectionAreaScript.Angle = value;
            }
        }
        public float DetectionRange
        {
            get
            {
                return detectionAreaScript.Range;
            }
            set
            {
                detectionAreaScript.Range = value;
            }
        }
        public EnemyBehavior Behavior { get; set; }
        public Vector3 LastPlayerKnownPosition { get; set; }
        public Vector3 LastPlayerKnownDirection { get; set; }
        public List<Vector3> Path { get; set; }

        private readonly EnemyDetectionAreaScript detectionAreaScript;

        public Enemy(GameObject o)
        {
            Behavior = EnemyBehavior.Scouting;
            GameObject = o;
            detectionAreaScript = GameObject.GetComponentInChildren<EnemyDetectionAreaScript>();
        }

        public Enemy(GameObject o, float detectionAngle) : this(o)
        {
            DetectionAngle = detectionAngle;
        }

        public Enemy(GameObject o, float detectionAngle, float detectionRange) : this(o, detectionAngle)
        {
            DetectionRange = detectionRange;
        }

        public Vector3 GetStartingPositon()
        {
            return Path[0];
        }

        public void Start()
        {
            Path = Utility.GetRandomNavMeshPath();
        }
    }
}
