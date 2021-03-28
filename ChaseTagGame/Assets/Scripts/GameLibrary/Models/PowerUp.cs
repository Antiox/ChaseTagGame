using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public class PowerUp
    {
        public PowerUpType Type { get; set; }
        public PowerUpFaction Faction { get; set; }
        public float Duration { get; set; }


        public PowerUp(PowerUpType type, PowerUpFaction faction)
        {
            Type = type;
            Duration = (int)type;
            Faction = faction;
        }


        public virtual void ExtendDuration(float duration)
        {
            Duration += duration;
        }

        public virtual void StartEffects(GameObject entity)
        {
            ToggleEffects(entity, true);
        }

        public virtual void StopEffects(GameObject entity)
        {
            Duration = 0;
            ToggleEffects(entity, false);
        }

        public override string ToString()
        {
            return $"{Type} - {Duration}";
        }




        private void ToggleEffects(GameObject entity, bool active)
        {
            var effect = entity.transform.Find($"{Type}Effect");
            if (effect != null)
                effect.gameObject.SetActive(active);
        }
    }
}
 