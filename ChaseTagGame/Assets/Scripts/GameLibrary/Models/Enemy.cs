using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class Enemy
    {
        public GameObject gameObject { get; set; }

        public Enemy(GameObject o)
        {
            gameObject = o;
        }
    }
}
