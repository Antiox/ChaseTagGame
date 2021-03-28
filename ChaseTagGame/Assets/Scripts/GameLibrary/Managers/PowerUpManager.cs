using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class PowerUpManager : Singleton<PowerUpManager>
    {
        public Dictionary<GameObject, List<PowerUp>> Entities { get; set; } = new Dictionary<GameObject, List<PowerUp>>();



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

        public void AddEntity(GameObject entity)
        {
            Entities.Add(entity, new List<PowerUp>());
        }

        public void AddPowerUpToEntity(GameObject entity, PowerUp powerUp)
        {
            if (!Entities.ContainsKey(entity))
                AddEntity(entity);

            var powerUps = Entities[entity];

            var existingPowerUp = powerUps.Find(p => p.Type == powerUp.Type);
            if (existingPowerUp == null)
            {
                if(powerUp.Type == PowerUpType.Clock)
                    ((ClockPowerUp)powerUp).StartEffects(entity);
                else
                    powerUp.StartEffects(entity);

                powerUps.Add(powerUp);
            }
            else
                existingPowerUp.ExtendDuration((int)powerUp.Type);
        }

        public PowerUp GetRandom()
        {
            var allBuffs = GetAllPowerUps();
            return allBuffs[UnityEngine.Random.Range(0, allBuffs.Count)];
        }

        public PowerUp GetRandom(PowerUpFaction faction)
        {
            var allBuffs = GetAllPowerUps();
            var factionBuffs = allBuffs.Where(p => p.Faction == faction).ToList();
            return factionBuffs[UnityEngine.Random.Range(0, factionBuffs.Count)];
        }


        private List<PowerUp> GetAllPowerUps()
        {
            var buffs = new List<PowerUp>();

            buffs.Add(new PowerUp(PowerUpType.Shield, PowerUpFaction.Allies));
            buffs.Add(new PowerUp(PowerUpType.Skull, PowerUpFaction.Enemy));
            buffs.Add(new ClockPowerUp(PowerUpType.Clock, PowerUpFaction.Allies, 15));

            return buffs;
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


    public enum PowerUpType
    {
        Shield = 5,
        Skull = 10,
        Clock = -1
    }

    public enum PowerUpFaction
    {
        Enemy = 1,
        Allies = 2
    }
}
