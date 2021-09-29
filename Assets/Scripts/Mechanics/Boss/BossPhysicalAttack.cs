using Interfaces;
using Mechanics.Collectibles;
using Mechanics.Powerups;
using Mechanics.Tanks.Movement;
using UnityEngine;

namespace Mechanics.Boss
{
    [RequireComponent(typeof(Collider))]
    public class BossPhysicalAttack : MonoBehaviour
    {
        [SerializeField] private BossFeedback _feedback;
        [SerializeField] private int _damageOnTouch = 4;
        [SerializeField] private float _pushBackForce = 4;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            if (_collider == null) {
                _collider = gameObject.AddComponent<BoxCollider>();
            }
            _collider.isTrigger = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            CollectibleBase collectible = other.gameObject.GetComponent<CollectibleBase>();
            if (collectible != null) {
                Destroy(other.gameObject);
                return;
            }
            PowerupBase powerup = other.gameObject.GetComponent<PowerupBase>();
            if (powerup != null) {
                Destroy(other.gameObject);
                return;
            }
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.OnTankImpact(_damageOnTouch);
                TankMovement movement = other.gameObject.GetComponentInChildren<TankMovement>();
                Vector3 angle = other.transform.position - transform.position;
                if (_feedback != null) _feedback.DamageAttack(angle);
                if (movement != null) {
                    movement.SetForce(angle.normalized * _pushBackForce);
                } else {
                    Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                    if (rb != null) {
                        rb.velocity += angle.normalized * _pushBackForce;
                    }
                }
            }
        }
    }
}