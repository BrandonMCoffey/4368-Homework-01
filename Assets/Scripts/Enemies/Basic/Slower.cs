using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Enemies.Basic {
    public class Slower : Enemy {
        [SerializeField] private ValueAdjustType _speedIncreaseType = ValueAdjustType.Multiply;
        [SerializeField] private float _amount = 2f;
        [SerializeField] private int _duration = 5;

        protected override bool OnImpact(GameObject other)
        {
            IMoveable moveableObject = other.GetComponent<IMoveable>();
            if (moveableObject == null) {
                return false;
            }
            // Slow down other object
            moveableObject.OnSpeedDecrease(_amount, _duration, _speedIncreaseType);

            return true;
        }
    }
}