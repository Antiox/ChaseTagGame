using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class ClockPowerUp : PowerUp
    {
        public double Amount { get; set; }

        public ClockPowerUp(PowerUpType type, PowerUpFaction faction, double amount) : base(type, faction) 
        {
            Amount = amount;
        }


        public override void StartEffects(GameObject entity)
        {
            base.StartEffects(entity);
            var e = new OnTimeAddedEvent(Amount);
            EventManager.Instance.Broadcast(e);
        }
    }
}
