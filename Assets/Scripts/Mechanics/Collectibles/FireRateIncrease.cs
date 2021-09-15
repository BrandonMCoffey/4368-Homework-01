using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Collectibles
{
    public class FireRateIncrease : CollectibleBase
    {
        [SerializeField] private float _fireRateIncrease = 2;

        protected override bool OnCollect(GameObject other)
        {
            // TODO: How to find IIncreaseable on a tank. Maybe have reference set up on Tank / Enemy?
            // Or a separate interface for each type of effect **
            IIncreaseable increasedFireRate = other.GetComponent<IIncreaseable>();
            if (increasedFireRate == null) {
                return false;
            }
            increasedFireRate.Increase(_fireRateIncrease);

            return true;
        }
    }
}