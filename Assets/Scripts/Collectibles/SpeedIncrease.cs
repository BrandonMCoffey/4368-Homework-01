using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class SpeedIncrease : CollectibleBase
    {
        [Header("Effect Settings")]
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.AddBase;
        [SerializeField] private float _amount = 1f;

        protected override bool OnCollect(GameObject other)
        {
            IMoveable moveableObject = other.GetComponent<IMoveable>();
            if (moveableObject == null) {
                return false;
            }
            // Permanently Increase the speed of the other object
            moveableObject.OnSpeedIncrease(_amount, -1, _speedIncreaseType);

            return true;
        }

        protected override void Movement(Transform obj)
        {
            Quaternion turnOffset = Quaternion.Euler(RotationSpeed, RotationSpeed, RotationSpeed);
            obj.rotation *= turnOffset;
        }
    }
}