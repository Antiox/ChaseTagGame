using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public static class Utility
    {
        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        public static IEnumerator Invoke(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static Vector3 GetBallisticForce(Vector3 source, Vector3 target, float angle, Vector3 gravity)
        {
            var direction = target - source;
            var h = direction.y;
            direction.y = 0;
            var distance = direction.magnitude;
            var a = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);

            var velocity = Mathf.Sqrt(distance * gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * direction.normalized;
        }

        public static Vector3 GetRandomTorque(float maxTorque)
        {
            var x = UnityEngine.Random.Range(-maxTorque, maxTorque);
            var y = UnityEngine.Random.Range(-maxTorque, maxTorque);
            var z = UnityEngine.Random.Range(-maxTorque, maxTorque);
            return new Vector3(x, y, z);
        }
    }
}
