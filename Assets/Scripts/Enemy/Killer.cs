using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class Killer : EnemyBase {
        protected override void PlayerImpact(Player player)
        {
            player.Kill();
        }
    }
}