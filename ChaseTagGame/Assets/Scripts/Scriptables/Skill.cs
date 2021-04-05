using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Scriptables/Skill")]
    public class Skill : ScriptableObject
    {
        public new string name;
        public Sprite artwork;
        public string description;
    }
}