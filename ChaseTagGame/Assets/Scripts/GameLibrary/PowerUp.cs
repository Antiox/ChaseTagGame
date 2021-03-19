using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLibrary
{
    public enum PowerUpType
    {
        Shield = 5,
    }

    public class PowerUp
    {
        public PowerUpType Type { get; set; }
        public float Duration { get; set; }


        public PowerUp(PowerUpType type)
        {
            Type = type;
            Duration = (int)type;
        }


        public void ExtendDuration(float duration)
        {
            Duration += duration;
        }

        public void StartEffects(GameObject entity)
        {
            ToggleEffects(entity, true);
        }

        public void StopEffects(GameObject entity)
        {
            ToggleEffects(entity, false);
        }

        private void ToggleEffects(GameObject entity, bool active)
        {
            var effect = entity.transform.Find($"{Type}Effect");
            if (effect != null)
                effect.gameObject.SetActive(active);
        }

        public override string ToString()
        {
            return $"{Type} - {Duration}";
        }
    }
}
 