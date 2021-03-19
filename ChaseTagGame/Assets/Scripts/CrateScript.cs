using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour
{
    public GameObject explosionParticles;
    public GameObject powerUp;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else
        {
            var explosionInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Instantiate(powerUp, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosionInstance, 3);
        }
    }
}
