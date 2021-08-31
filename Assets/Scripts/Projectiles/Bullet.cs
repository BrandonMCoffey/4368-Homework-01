using System.Collections;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Player;
using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.Projectiles {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Bullet : MonoBehaviour {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _bounceTimes = 1;

        private Rigidbody _rb;
        private Collider _collider;
        private int _currentBounces;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _collider.isTrigger = false;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnCollisionEnter(Collision other)
        {
            IDamageable health = other.gameObject.GetComponent<TankHealth>();
            if (health != null) {
                health.OnBulletImpact(_damageAmount);
                Kill();
            } else if (_currentBounces++ < _bounceTimes) {
                var direction = transform.forward;

                Ray ray = new Ray(transform.position, direction);
                if (Physics.Raycast(ray, out var hit, 10)) {
                    direction = Vector3.Reflect(direction, hit.normal);
                }
                transform.forward = direction;
            } else {
                Bullet otherBullet = other.gameObject.GetComponent<Bullet>();
                if (otherBullet != null) {
                    otherBullet.Kill();
                }
                Kill();
            }
        }

        public void TemporaryIgnore(Collider objCollider, float duration)
        {
            StartCoroutine(Ignore(objCollider, duration));
        }

        private IEnumerator Ignore(Collider objCollider, float duration)
        {
            Physics.IgnoreCollision(_collider, objCollider, true);
            yield return new WaitForSecondsRealtime(duration);
            Physics.IgnoreCollision(_collider, objCollider, false);
        }

        private void Move()
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }

        private void Kill()
        {
            Destroy(gameObject);
        }
    }
}