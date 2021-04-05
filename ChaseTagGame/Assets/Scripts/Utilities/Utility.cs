using System;
using System.Collections;
using System.Collections.Generic;
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
            var maxIndices = navMeshData.indices.Length - 3;
            var firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
            var secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);
            var firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
            var secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];

            if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
                return GetRandomNavMeshPosition();
            else
                return Vector3.Lerp(firstVertexPosition, secondVertexPosition, UnityEngine.Random.Range(0.05f, 0.95f));
        }
    }
}
