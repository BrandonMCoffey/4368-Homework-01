using Interfaces;
using UnityEngine;

namespace Mechanics.Collectibles
{
    public class HealthIncrease : CollectibleBase
    {
        [Header("Effect Settings")]
        [SerializeField] private int _healthAdded = 1;

        protected override bool OnCollect(GameObject other)
        {
            IHealable healableObject = other.GetComponent<IHealable>();
            if (healableObject == null) {
                return false;
            }
            // Heal the other object
            return healableObject.OnHeal(_healthAdded);
        }
    }
}