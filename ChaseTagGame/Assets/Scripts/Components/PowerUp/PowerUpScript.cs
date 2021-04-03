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
        var allowedForPlayer = other.CompareTag(GameTags.Player) && powerUp.Faction == PowerUpFaction.Allies;
        var allowedForEnemies = other.CompareTag(GameTags.Enemy) && powerUp.Faction == PowerUpFaction.Enemy;

        if (allowedForPlayer || allowedForEnemies)
        {
            var e = new OnPowerUpTriggerEnterEvent(gameObject, other.gameObject);
            EventManager.Instance.Dispatch(e);
        }
    }
}
