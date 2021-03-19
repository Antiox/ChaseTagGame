using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class PowerUpScript : MonoBehaviour
{
    public PowerUpType type;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameScript>().NotifyPowerUpTriggerEnter(other, gameObject);
    }
}
