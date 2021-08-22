using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class Slower : EnemyBase {
        [SerializeField] private float _speedMultiplier = -0.5f;
        [SerializeField] private float _duration = 5;

        protected override void PlayerImpact(Player player)
        {
            // TODO: Do not decrease speed to 0. Divide downwards not subtract
            player.AdjustSpeed(_speedMultiplier, _duration);
        }
    }
}