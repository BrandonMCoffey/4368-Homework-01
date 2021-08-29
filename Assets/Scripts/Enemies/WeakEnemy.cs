using Assets.Scripts.Player;
using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.Enemies {
    [RequireComponent(typeof(TankHealth))]
    public class WeakEnemy : Enemy {
        protected override void PlayerImpact(PlayerTank playerTank)
        {
            base.PlayerImpact(playerTank);
            //Health.DecreaseHealth(1);
        }

        public override void Kill()
        {
            Instantiate(ImpactParticles, transform.position, Quaternion.identity);
            base.Kill();
        }
    }
}