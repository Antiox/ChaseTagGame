using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 1)]
    public class LightingPreset : ScriptableObject
    {
#pragma warning disable S1104 // Fields should not have public accessibility
        public Gradient AmbientColor;
        public Gradient DirectionalColor;
        public Gradient FogColor;
#pragma warning restore S1104 // Fields should not have public accessibility
    }
}
