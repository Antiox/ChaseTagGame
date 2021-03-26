using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public static class Extensions
    {
        #region Rigidbody
        public static bool IsGrounded(this Rigidbody body, float groundCheckRadius)
        {
            return Physics.CheckSphere(body.transform.position + ((groundCheckRadius - 0.05f) * body.transform.up), groundCheckRadius, body.gameObject.layer, QueryTriggerInteraction.Ignore);
        }

        public static bool IsFacingWall(this Rigidbody body, Transform castTransform)
        {
            return GetAngleFromWall(body, castTransform) <= 40;
        }

        public static float GetAngleFromWall(this Rigidbody body, Transform castTransform)
        {
            if (Physics.Raycast(castTransform.position, castTransform.forward, out var hit, 0.7f, body.gameObject.layer))
                return Vector3.Angle(hit.normal, -castTransform.forward);

            return 999;
        }

        public static Vector3 GetFacingWallNormal(this Rigidbody body, Transform castTransform)
        {
            if (Physics.Raycast(castTransform.position, castTransform.forward, out var hit, 0.7f, body.gameObject.layer))
                return hit.normal;

            return Vector3.zero;
        }

        public static Slope GetSlope(this Rigidbody body, Vector3 forwardDirection)
        {
            var slope = new Slope();
            if (Physics.Raycast(body.transform.position, Vector3.down, out var hitDown, 0.6f, body.gameObject.layer))
            {
                slope.Angle = Vector3.Angle(hitDown.normal, Vector3.up);
                slope.Normal = hitDown.normal;
            }

            if (slope.Angle == 0)
            {
                if (Physics.Raycast(body.transform.position, forwardDirection, out var hitForward, 0.4f, body.gameObject.layer))
                {
                    slope.Angle = Vector3.Angle(hitForward.normal, Vector3.up);
                    slope.Normal = hitDown.normal;

                    if (slope.Angle == 90)
                        slope = new Slope() { Angle = 0, Normal = Vector3.up };
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
            Debug.DrawRay(body.transform.position, Vector3.down * 1f, Color.red, Time.deltaTime);

            if (Physics.Raycast(body.transform.position, Vector3.down, out var hit, 0.6f, body.gameObject.layer))
                return hit.normal != Vector3.up;

            return false;
        }

        public static float GetSlopeAngle(this Rigidbody body)
        {
            if (Physics.Raycast(body.transform.position, Vector3.down, out var hit, 0.6f, body.gameObject.layer))
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
        #endregion
    }
}
