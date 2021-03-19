using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class PowerUpScript : MonoBehaviour
{
    public PowerUpType type;
    private GameObject gameManager;


    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag(GameTags.GameManager);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameScript>().NotifyPowerUpTriggerEnter(other, gameObject);
    }
}
