using Assets.Scripts.Player;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Enemies {
    public class Slower : Enemy {
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.Multiply;
        [SerializeField] private float _amount = 0.5f;
        [SerializeField] private int _duration = 5;

        protected override void PlayerImpact(PlayerTank playerTank)
        {
            AdjustableFloat speed = playerTank.GetTankController().AdjustMoveSpeed;
            StartCoroutine(speed.AdjustValueOverTime(_speedIncreaseType, _amount, _duration));
        }
    }
}