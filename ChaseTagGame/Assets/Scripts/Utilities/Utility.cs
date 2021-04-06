using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace GameLibrary
{
    public static class Utility
    {
        public static bool ApproximatelyEquals(float a, float b)
        {
            return Math.Abs(a - b) < 0.0001;
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

        public static Vector3 GetRandomNavMeshPosition()
        {
            var navMeshData = NavMesh.CalculateTriangulation();
            var randomVertex = UnityEngine.Random.Range(0, navMeshData.vertices.Length);
            var randomPoint = navMeshData.vertices[randomVertex] + UnityEngine.Random.insideUnitSphere * 20f;
            NavMesh.SamplePosition(randomPoint, out var hit, 20f, 1);
            NavMesh.FindClosestEdge(hit.position, out var edge, 1);

            if (hit.position == edge.position)
                return GetRandomNavMeshPosition();

            return hit.position;
        }
    }
}
