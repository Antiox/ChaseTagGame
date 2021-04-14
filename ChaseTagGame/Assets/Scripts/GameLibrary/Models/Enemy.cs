using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class Enemy
    {
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


        private readonly EnemyDetectionAreaScript detectionAreaScript;


        public Enemy(GameObject o)
        {
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
    }
}
