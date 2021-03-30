using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class SafeZoneScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(GameTags.Player))
            {
                var e = new OnPlayerEnterSafeZoneEvent(other.gameObject);
                EventManager.Instance.Broadcast(e);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GameTags.Player))
            {
                var e = new OnPlayerExitSafeZoneEvent(other.gameObject);
                EventManager.Instance.Broadcast(e);
            }
        }
    }
}
