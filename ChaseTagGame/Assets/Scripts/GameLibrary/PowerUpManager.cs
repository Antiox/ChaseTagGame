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
            {
                var p = new PowerUp(type);
                p.StartEffects(entity);
                powerUps.Add(p);
            }
            else
                existingPowerUp.ExtendDuration((int)type);
        }

        public void Update()
        {
            UpdateDurations();
        }

        public string GetAllEntityPowerUps(GameObject entity)
        {
            if (!Entities.ContainsKey(entity))
                return "";

            return string.Join(" | ", Entities[entity]);
        }

        public bool IsEntityShielded(GameObject entity)
        {
            if (!Entities.ContainsKey(entity))
                return false;

            return Entities[entity].Find(p => p.Type == PowerUpType.Shield) != null;
        }

        public void RemovePowerUpFromEntity(GameObject entity, PowerUpType type)
        {
            var powerUp = Entities[entity].Find(p => p.Type == type);
            powerUp.StopEffects(entity);
        }

        private void UpdateDurations()
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                var e = Entities.ElementAt(i);
                for (int j = e.Value.Count - 1; j >= 0; j--)
                {
                    var p = e.Value[j];

                    p.Duration -= Time.deltaTime;
                    if (p.Duration <= 0)
                    {
                        Entities[e.Key].Remove(p);
                        p.StopEffects(e.Key);
                    }
                }

                if (e.Value.Count == 0)
                    Entities.Remove(e.Key);
            }
        }

    }
}
