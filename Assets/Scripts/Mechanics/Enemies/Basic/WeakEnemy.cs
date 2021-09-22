using Interfaces;
using Mechanics.Tanks;
using UnityEngine;

namespace Mechanics.Enemies.Basic
{
    [RequireComponent(typeof(TankHealth))]
    public class WeakEnemy : Enemy
    {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _selfDamageAmount = 1;

        private TankHealth _health;

        protected override void Awake()
        {
            base.Awake();
            _health = GetComponent<TankHealth>();
        }

        protected override bool OnImpact(GameObject other)
        {
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            if (damageableObject == null) {
                return false;
            }
            // Deal damage to other object
            damageableObject.OnTankImpact(_damageAmount);
            // Deal damage to self
            _health.OnTankImpact(_selfDamageAmount);

            return true;
        }
    }
}