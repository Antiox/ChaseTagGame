using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour
{
    public GameObject explosionParticles;
    public List<GameObject> PossiblePowerUps;

    public IPowerUp powerUp { get; set; }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(GameTags.Wall) || collision.collider.CompareTag(GameTags.PowerUp))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else
        {
            var explosionInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            var powerUpGameObject = PossiblePowerUps.Find(p => p.name == $"{powerUp.Type}PowerUp");
            var p = Instantiate(powerUpGameObject, transform.position, Quaternion.identity);
            p.GetComponent<PowerUpScript>().powerUp = powerUp;

            Destroy(gameObject);
            Destroy(explosionInstance, 3);
        }
    }
}
