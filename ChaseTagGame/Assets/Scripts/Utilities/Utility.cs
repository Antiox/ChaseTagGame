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

        public static List<Vector3> GetRandomNavMeshPath()
        {
            var path = new List<Vector3>();

            for (int i = 0; i < UnityEngine.Random.Range(5f, 10f); i++)
            {
                path.Add(GetRandomNavMeshPosition());
            }

            return path;
        }

        public static List<Vector3> GetRandomNavMeshCircularPath()
        {
            var path = new List<Vector3>();
            var pointOffset = UnityEngine.Random.Range(1.5f, 4f);
            var points = UnityEngine.Random.Range(3, 15);
            var radius = UnityEngine.Random.Range(3f, 6f);
            var direction = UnityEngine.Random.value > 0.5f ? -1 : 1;
            var center = GetRandomNavMeshPosition();

            for (int i = 0; i < points; i++)
            {
                var angle = Mathf.Deg2Rad * (360 / points) * i * direction;
                var pointOnCircle = center + new Vector3(radius * Mathf.Sin(angle), 0, radius * Mathf.Cos(angle));
                var randomWayPoint = pointOnCircle + UnityEngine.Random.insideUnitSphere * pointOffset;
                NavMesh.SamplePosition(randomWayPoint, out var hit, pointOffset * 2, 1);
                path.Add(hit.position);
            }

            return path;
        }

        public static Vector3 GetNavMeshCenter()
        {
            return NavMesh.CalculateTriangulation().vertices.Average();
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
