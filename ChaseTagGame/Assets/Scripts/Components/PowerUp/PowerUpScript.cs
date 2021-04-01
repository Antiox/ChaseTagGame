using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLibrary;

public class PowerUpScript : MonoBehaviour
{
    public PowerUpType type;
    public PowerUpFaction faction;

    public IPowerUp powerUp { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        var e = new OnPowerUpTriggerEnterEvent(gameObject, other.gameObject);
        EventManager.Instance.Dispatch(e);
    }
}
