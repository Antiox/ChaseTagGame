using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using GameLibrary;

namespace ExtensionClass
{
    public static class Extensions
    {
        public static bool IsGrounded(this Rigidbody body, float groundCheckRadius)
        {
            return GetColliders(body, groundCheckRadius).Length > 0;
        }

        public static Slope GetSlope(this Rigidbody body, Vector3 forwardDirection)
        {
            var slope = new Slope();
            RaycastHit hitDown;

            if (Physics.Raycast(body.transform.position, Vector3.down, out hitDown, 0.6f, body.gameObject.layer))
            {
                slope.Angle = Vector3.Angle(hitDown.normal, Vector3.up);
                slope.Normal = hitDown.normal;
            }

            if (slope.Angle == 0)
            {
                RaycastHit hitForward;
                if (Physics.Raycast(body.transform.position, forwardDirection, out hitForward, 0.4f, body.gameObject.layer))
                {
                    slope.Angle = Vector3.Angle(hitForward.normal, Vector3.up);
                    slope.Normal = hitDown.normal;
                }
            }

            return slope;
        }

        public static Collider[] GetColliders(this Rigidbody body, float groundCheckRadius)
        {
            var hitPoints = Physics.OverlapSphere(body.transform.position + ((groundCheckRadius - 0.05f) * body.transform.up), groundCheckRadius, body.gameObject.layer);
            return hitPoints;
        }

        public static bool CanWalkOnSlope(this Rigidbody body, Slope slope, float maxAngle)
        {
            return slope.Angle <= maxAngle;
        }

        public static bool IsOnSlope(this Rigidbody body)
        {
            RaycastHit hit;
            Debug.DrawRay(body.transform.position, Vector3.down * 1f, Color.red, Time.deltaTime);

            if (Physics.Raycast(body.transform.position, Vector3.down, out hit, 0.6f , body.gameObject.layer))
                return hit.normal != Vector3.up;

            return false;
        }

        public static float GetSlopeAngle(this Rigidbody body)
        {
            RaycastHit hit;

            if (Physics.Raycast(body.transform.position, Vector3.down, out hit, 0.6f, body.gameObject.layer))
                return Vector3.Angle(hit.normal, Vector3.up);

            return -1;
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
