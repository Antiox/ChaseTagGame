using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class OnPowerUpSpawnRequestedEvent : IGameEvent
    {
        public PowerUpFaction Faction { get; set; }


        public OnPowerUpSpawnRequestedEvent()
        {
            var factions = Enum.GetValues(typeof(PowerUpFaction));
            Faction = (PowerUpFaction)factions.GetValue(UnityEngine.Random.Range(0, factions.Length));
        }

        public OnPowerUpSpawnRequestedEvent(PowerUpFaction faction)
        {
            Faction = faction;
        }
    }
}
