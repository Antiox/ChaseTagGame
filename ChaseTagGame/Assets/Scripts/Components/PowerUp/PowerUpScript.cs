using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class PowerUpScript : MonoBehaviour
{
    public PowerUpType type;
    public PowerUpFaction faction;

    private void OnTriggerEnter(Collider other)
    {
        var e = new OnPowerUpTriggerEnterEvent(gameObject, other.gameObject);
        EventManager.Instance.Broadcast(e);
    }
}
