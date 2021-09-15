using System.Collections;
using Assets.Scripts.Audio;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private BulletType _type = BulletType.Normal;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _bounceTimes = 1;
        [Header("Feedback")]
        [SerializeField] private SfxReference _fireSfx = new SfxReference();
        [SerializeField] private ParticleSystem _fireParticles = null;
        [SerializeField] private SfxReference _bounceSfx = new SfxReference();
        [SerializeField] private ParticleSystem _bounceParticles = null;
        [SerializeField] private SfxReference _destroySfx = new SfxReference();
        [SerializeField] private ParticleSystem _destroyParticles = null;

        public BulletType Type => _type;

        private Rigidbody _rb;
        private Collider _collider;
        private int _currentBounces;

        private ReflectionDebug _debug;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _collider.isTrigger = false;

            _debug = GetComponent<ReflectionDebug>();
            if (_debug != null) _debug.ReflectionTimes = _bounceTimes + 1;

            // Ensure collect particles don't play on awake or self destruct
            if (_fireParticles != null && !_fireParticles.gameObject.activeInHierarchy) {
                DebugHelper.Warn(gameObject, "Bullet Fire Particles should be under the Bullet Prefab");
            }
            if (_bounceParticles != null && !_bounceParticles.gameObject.activeInHierarchy) {
                DebugHelper.Warn(gameObject, "Bullet Bounce Particles should be under the Bullet Prefab");
            }
            if (_destroyParticles != null && _destroyParticles.gameObject.activeInHierarchy) {
                _destroyParticles.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _currentBounces = 0;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnCollisionEnter(Collision other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null) {
                // Damage the object and kill bullet
                if (damageable.OnBulletImpact(_damageAmount)) {
                    Kill();
                    return;
                }
            }
            // If can bounce, bounce
            if (_currentBounces++ < _bounceTimes) {
                if (_debug != null) _debug.ReflectionTimes = _bounceTimes - _currentBounces + 1;

                var direction = transform.forward;

                Ray ray = new Ray(transform.position, direction);
                if (Physics.Raycast(ray, out var hit, 10)) {
                    direction = Vector3.Reflect(direction, hit.normal);
                }
                transform.forward = direction;
                BounceFeedback(hit.point, Quaternion.Euler(hit.normal));
                return;
            }
            // If hit another bullet, kill both
            Bullet otherBullet = other.gameObject.GetComponent<Bullet>();
            if (otherBullet != null) {
                otherBullet.Kill();
            }
            Kill();
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

        public void FireFeedback(Vector3 position, Quaternion rotation)
        {
            if (_fireParticles != null) {
                _fireParticles.transform.SetPositionAndRotation(position, rotation);
                _fireParticles.Play();
            }
            _fireSfx.Play();
        }

        public void BounceFeedback(Vector3 position, Quaternion rotation)
        {
            if (_bounceParticles != null) {
                _bounceParticles.gameObject.SetActive(true);
                _bounceParticles.transform.SetPositionAndRotation(position, rotation);
                _bounceParticles.Play();
            }
            _bounceSfx.Play();
        }

        public void DestroyFeedback(Vector3 position, Quaternion rotation)
        {
            if (_destroyParticles != null) {
                Instantiate(_destroyParticles, position, rotation).gameObject.SetActive(true);
            }
            _destroySfx.Play();
        }

        private void Kill()
        {
            DestroyFeedback(transform.position, transform.rotation);
            BulletPool.Instance.ReturnBullet(this);
        }
    }
}