using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    public class Invincibility : PowerupBase {
        protected override void ActivatePowerup(EntityHealth health)
        {
            base.ActivatePowerup(health);
            health.SetInvincible();
        }

        protected override void DeactivatePowerup(EntityHealth health)
        {
            base.DeactivatePowerup(health);
            health.RemoveInvincible();
        }
    }
}