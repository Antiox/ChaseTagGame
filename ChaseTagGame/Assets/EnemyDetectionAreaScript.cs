using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GameLibrary
{
    public class EnemyDetectionAreaScript : MonoBehaviour
    {
        [SerializeField, Range(0f, 360f)] private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        [SerializeField, Range(0f, 10f)] private float range;
        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        [SerializeField] private UnityEvent<Collider> onPlayerInDetectionArea;

        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;


        private void Start()
        {
            StartCoroutine(CheckDetectionArea());
        }


        private void OnDrawGizmos()
        {
            var viewAngleA = Utility.GetDirectionFromAngle(-Angle / 2);
            var viewAngleB = Utility.GetDirectionFromAngle(Angle / 2);

            Handles.color = Color.red;
            Handles.DrawWireArc(transform.position, Vector3.up, viewAngleA, Angle, Range);
            Handles.DrawLine(transform.position, transform.position + viewAngleA * Range);
            Handles.DrawLine(transform.position, transform.position + viewAngleB * Range);


            Gizmos.DrawWireSphere(transform.position, Range);
        }

        private IEnumerator CheckDetectionArea()
        {
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, Range, targetMask);

                foreach (var c in colliders)
                {
                    var targetPosition = c.transform.position + Vector3.up;
                    var directionToTarget = (targetPosition - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2f)
                    {
                        var distanceToTarget = Vector3.Distance(targetPosition, transform.position);

                        if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                            continue;

                        onPlayerInDetectionArea.Invoke(c);
                    }
                }

                yield return new WaitForFixedUpdate();
            }

        }

        private void DrawDetectionArea()
        {

        }
    }
}