using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class SpeedIncrease : CollectibleBase {
        [SerializeField] private int _speedMultiplier = 1;
        [SerializeField] private int _increaseDuration = 2;

        protected override void Collect(Player player)
        {
            player.AdjustSpeed(_speedMultiplier, _increaseDuration);
        }

        protected override void Movement(Rigidbody rb)
        {
            Quaternion turnOffset = Quaternion.Euler(MovementSpeed, MovementSpeed, MovementSpeed);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}