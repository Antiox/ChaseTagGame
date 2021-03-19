using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class PowerUpManager
    {
        public Dictionary<GameObject, List<PowerUp>> Entities { get; set; } = new Dictionary<GameObject, List<PowerUp>>();

        public PowerUpManager()
        {
        }

        public void AddEntity(GameObject entity)
        {
            Entities.Add(entity, new List<PowerUp>());
        }

        public void AddPowerUpToEntity(GameObject entity, PowerUpType type)
        {
            if (!Entities.ContainsKey(entity))
                AddEntity(entity);

            var powerUps = Entities[entity];

            var existingPowerUp = powerUps.Find(p => p.Type == type);
            if(existingPowerUp == null)
                powerUps.Add(new PowerUp(type));
            else
                existingPowerUp.ExtendDuration((int)type);
        }

        public void UpdateDurations(float delta)
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                var e = Entities.ElementAt(i);
                for (int j = e.Value.Count - 1; j >= 0; j--)
                {
                    var p = e.Value[j];

                    p.Duration -= delta;
                    if (p.Duration <= 0)
                        Entities[e.Key].Remove(p);
                }

                if (e.Value.Count == 0)
                    Entities.Remove(e.Key);
            }
        }

        public string GetAllEntityPowerUps(GameObject entity)
        {
            if (!Entities.ContainsKey(entity))
                return "Entity doesn't exist";

            return string.Join(" | ", Entities[entity]);
        }
    }
}
