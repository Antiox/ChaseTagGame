using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtensionClass
{
    public static class Extensions
    {
        public static bool IsGrounded(this Rigidbody body, float distanceToGround)
        {
            return Physics.CheckSphere(body.position, distanceToGround, body.gameObject.layer);
        }

        public static void Jump(this Rigidbody body, float jumpHeight)
        {
            body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight), ForceMode.VelocityChange);
        }

        public static void Slide(this Rigidbody body, float slideForce)
        {
            body.AddForce(body.transform.forward * slideForce, ForceMode.Acceleration);
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
