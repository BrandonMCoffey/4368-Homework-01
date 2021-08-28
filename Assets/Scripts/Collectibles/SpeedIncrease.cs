using Assets.Scripts.PlayerTank;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class SpeedIncrease : CollectibleBase {
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.AddBase;
        [SerializeField] private float _amount = 1f;
        [SerializeField] private bool _overTime = false;
        [SerializeField] private int _duration = 0;

        protected override bool Collect(Player player)
        {
            if (_overTime && _duration >= 0) {
                AdjustableFloat speed = player.GetTankController().AdjustMoveSpeed;
                StartCoroutine(speed.AdjustValueOverTime(_speedIncreaseType, _amount, _duration));
            } else {
                player.GetTankController().AdjustMoveSpeed.IncreaseValue(_speedIncreaseType, _amount);
            }
            return true;
        }

        protected override void Movement(Rigidbody rb)
        {
            Quaternion turnOffset = Quaternion.Euler(MovementSpeed, MovementSpeed, MovementSpeed);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}