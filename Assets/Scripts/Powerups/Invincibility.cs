using Assets.Scripts.Tanks;

namespace Assets.Scripts.Powerups {
    public class Invincibility : PowerupBase {
        protected override void ActivatePowerup(TankHealth health)
        {
            base.ActivatePowerup(health);
            health.SetInvincible();
        }

        protected override void DeactivatePowerup(TankHealth health)
        {
            base.DeactivatePowerup(health);
            health.RemoveInvincible();
        }
    }
}