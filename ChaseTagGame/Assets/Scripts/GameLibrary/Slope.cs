using UnityEngine;

namespace GameLibrary
{
    public class Slope
    {
        public float Angle { get; set; }
        public Vector3 Normal { get; set; }
        public bool IsOnSlope { get { return Normal != Vector3.up && Normal != Vector3.zero; } }
    }
}
