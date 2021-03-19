using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtensionClass
{
    public static class Extensions
    {
        public static void IncreaseGravity(this Rigidbody body, float factor)
        {
            if (!ApproximatelyEquals(Math.Abs(body.velocity.y), 0))
            {
                var velocity = body.velocity;
                velocity.y += Physics.gravity.y * factor * Time.deltaTime;
                body.velocity = velocity;
            }
        }

        public static bool IsGrounded(this Rigidbody body, float distanceToGround)
        {
            var startingPosition = body.transform.position;
            startingPosition.y += 0.5f;
            return Physics.Raycast(startingPosition, -Vector3.up, distanceToGround + 0.3f);
        }

        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        public static IEnumerator Invoke(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}
