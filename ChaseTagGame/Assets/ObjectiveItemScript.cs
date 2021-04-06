using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class ObjectiveItemScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(GameTags.Player))
            {
                var e = new OnObjectiveItemTriggerEnterEvent(other.gameObject, gameObject);
                EventManager.Instance.Dispatch(e);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag(GameTags.Enemy))
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
    }
}