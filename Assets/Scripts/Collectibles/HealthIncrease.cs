using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class HealthIncrease : CollectibleBase {
        [SerializeField] private int _healthAdded = 1;

        protected override bool Collect(PlayerTank playerTank)
        {
            bool healthUsed = playerTank.Health.IncreaseHealth(_healthAdded);
            return healthUsed;
        }
    }
}