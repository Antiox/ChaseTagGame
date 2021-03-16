using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtensionClass
{
    public static class Extensions
    {
        public static void IncreaseGravity(this Rigidbody body, float factor)
        {
            if (Math.Abs(body.velocity.y) > 0.0001)
            {
                var velocity = body.velocity;
                velocity.y += Physics.gravity.y * factor * Time.deltaTime;
                body.velocity = velocity;
            }
        }
    }
}
