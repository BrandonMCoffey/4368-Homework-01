using Assets.Scripts.Player;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class SpeedIncrease : CollectibleBase {
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.AddBase;
        [SerializeField] private float _amount = 1f;
        [SerializeField] private bool _overTime = false;
        [SerializeField] private int _duration = 0;

        protected override bool Collect(PlayerTank playerTank)
        {
            if (_overTime && _duration >= 0) {
                AdjustableFloat speed = playerTank.Movement.MoveSpeed;
                StartCoroutine(speed.AdjustValueOverTime(_speedIncreaseType, _amount, _duration));
            } else {
                playerTank.Movement.MoveSpeed.IncreaseValue(_speedIncreaseType, _amount);
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