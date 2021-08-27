using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    [RequireComponent(typeof(EntityHealth))]
    public class WeakEnemy : EnemyBase {
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