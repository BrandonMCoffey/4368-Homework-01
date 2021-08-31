using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    public class RapidFire : PowerupBase {
        [SerializeField] private float _effectMultiplication = 2;

        private IIncreaseable _effected;

        protected override bool OnCollect(GameObject other)
        {
            // TODO: How to find IIncreaseable on a tank. Maybe have reference set up on Tank / Enemy?
            // Or a separate interface for each type of effect **
            IIncreaseable increasedFireRate = other.GetComponent<IIncreaseable>();
            if (increasedFireRate == null) {
                return false;
            }
            _effected = increasedFireRate;

            return true;
        }


        protected override void Activate()
        {
            _effected.Increase(_effectMultiplication);
        }

        protected override void Deactivate()
        {
            //_effected.Decrease(_effectMultiplication);
        }
    }
}