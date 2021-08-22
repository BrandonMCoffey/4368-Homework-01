using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class HealthIncrease : CollectibleBase {
        [SerializeField] private int _healthAdded = 1;

        protected override void Collect(Player player)
        {
            player.GetHealth().IncreaseHealth(_healthAdded);
        }
    }
}