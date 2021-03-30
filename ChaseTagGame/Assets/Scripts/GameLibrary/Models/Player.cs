using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class Player
    {
        public GameObject gameObject { get; set; }
        public bool IsInSafeZone { get; set; }


        public Player(GameObject o)
        {
            gameObject = o;
        }
    }
}
