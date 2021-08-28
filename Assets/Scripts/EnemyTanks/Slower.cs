using Assets.Scripts.PlayerTank;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.EnemyTanks {
    public class Slower : Enemy {
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.Multiply;
        [SerializeField] private float _amount = 0.5f;
        [SerializeField] private int _duration = 5;

        protected override void PlayerImpact(Player player)
        {
            AdjustableFloat speed = player.GetTankController().AdjustMoveSpeed;
            StartCoroutine(speed.AdjustValueOverTime(_speedIncreaseType, _amount, _duration));
        }
    }
}