using Interfaces;
using UnityEngine;

namespace Mechanics.Collectibles
{
    public class DamageBonusPickup : CollectibleBase
    {
        [SerializeField] private int _damageBonus = 1;

        protected override bool OnCollect(GameObject other)
        {
            // TODO: How to find IIncreaseable on a tank. Maybe have reference set up on Tank / Enemy?
            // Or a separate interface for each type of effect **
            IIncreaseable damageBonus = other.GetComponent<IIncreaseable>();
            if (damageBonus == null) {
                return false;
            }
            damageBonus.Increase(_damageBonus);

            return true;
        }
    }
}