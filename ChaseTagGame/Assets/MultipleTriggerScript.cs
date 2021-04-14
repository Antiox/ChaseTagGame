using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameLibrary
{
    public class MultipleTriggerScript : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> function;

        private void OnTriggerEnter(Collider other)
        {
            function.Invoke(other);
        }
    }
}