using UnityEngine;

namespace Mechanics.Projectiles
{
    public class BulletMotor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [Header("References")]
        [SerializeField] private Rigidbody _rb;

        private bool _missingRigidbody;

        private void Awake()
        {
            if (_rb == null) {
                _rb = GetComponent<Rigidbody>();
                if (_rb == null) {
                    _missingRigidbody = true;
                    throw new MissingComponentException("Missing Rigidbody for Bullet on " + gameObject);
                }
            }
        }

        private void FixedUpdate()
        {
            if (_missingRigidbody) return;
            Move();
        }

        private void Move()
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }
    }
}