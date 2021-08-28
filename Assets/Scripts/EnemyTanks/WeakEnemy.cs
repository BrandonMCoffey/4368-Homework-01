using Assets.Scripts.PlayerTank;
using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.EnemyTanks {
    [RequireComponent(typeof(TankHealth))]
    public class WeakEnemy : Enemy {
        protected override void PlayerImpact(Player player)
        {
            base.PlayerImpact(player);
            Health.DecreaseHealth(1);
        }

        public override void Kill()
        {
            Instantiate(ImpactParticles, transform.position, Quaternion.identity);
            base.Kill();
        }
    }
}