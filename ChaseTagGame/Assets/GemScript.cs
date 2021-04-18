using UnityEngine;

namespace GameLibrary
{
    public class GemScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.Player))
            {
                var e = new OnGemTriggerEnterEvent(other.gameObject, gameObject);
                EventManager.Instance.Dispatch(e);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(GameTags.Enemy))
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
    }
}

