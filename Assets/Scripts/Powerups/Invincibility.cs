using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    public class Invincibility : PowerupBase {
        protected override void ActivatePowerup(EntityHealth health)
        {
            health.SetInvincible(PowerupDuration);
        }
    }
}