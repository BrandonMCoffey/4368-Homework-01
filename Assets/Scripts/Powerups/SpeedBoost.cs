using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    public class SpeedBoost : PowerupBase {
        [Header("Effect Settings")]
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.AddBase;
        [SerializeField] private float _amount = 1f;

        private IMoveable _effected;

        protected override bool OnCollect(GameObject other)
        {
            IMoveable moveableObject = other.GetComponent<IMoveable>();
            if (moveableObject == null) {
                return false;
            }
            _effected = moveableObject;

            return true;
        }


        protected override void Activate()
        {
            // Is it good practice to do this or use Deactivate?
            // The particle effects require the OnSpeedIncrease with duration to be used...
            // Maybe this is why collectible speed increase is better
            // But too much speed adds up. I prefer this way... 
            _effected.OnSpeedIncrease(_amount, Duration, _speedIncreaseType);
        }

        protected override void Deactivate()
        {
        }
    }
}