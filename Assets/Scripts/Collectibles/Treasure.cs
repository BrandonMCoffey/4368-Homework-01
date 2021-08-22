using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class Treasure : CollectibleBase {
        [SerializeField] private int value = 1;

        protected override void Collect(Player player)
        {
            player.GetInventory().AddTreasure(value);
        }
    }
}