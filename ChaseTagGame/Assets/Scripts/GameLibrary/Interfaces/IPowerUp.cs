
using UnityEngine;

namespace GameLibrary
{
    public interface IPowerUp
    {
        public PowerUpType Type { get; set; }
        public PowerUpFaction Faction { get; set; }
        public float Duration { get; set; }


        public void ExtendDuration(float duration);

        public void StartEffects(GameObject entity);

        public void StopEffects(GameObject entity);
    }
}
