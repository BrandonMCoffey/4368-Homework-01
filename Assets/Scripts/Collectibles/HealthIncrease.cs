using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class HealthIncrease : CollectibleBase {
        [SerializeField] private int _healthAdded = 1;

        protected override bool Collect(Player player)
        {
            bool healthUsed = player.Health.IncreaseHealth(_healthAdded);
            return healthUsed;
        }
    }
}