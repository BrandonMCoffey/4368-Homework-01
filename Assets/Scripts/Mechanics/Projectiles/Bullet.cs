using System.Collections;
using Interfaces;
using UnityEngine;

namespace Mechanics.Projectiles
{
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private BulletType _type = BulletType.Normal;
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _bounceTimes = 1;
        [Header("References")]
        [SerializeField] private Collider _collider;
        [SerializeField] private BulletFeedback _feedback;

        public BulletType Type => _type;

        private int _currentBounces;
        private bool _hasError;

        private ReflectionDebug _debug;

        private void Awake()
        {
            if (_collider == null) {
                _collider = GetComponent<Collider>();
                if (_collider == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Rigidbody for Bullet on " + gameObject);
                }
            }
            if (_feedback == null) {
                _feedback = transform.parent.GetComponentInChildren<BulletFeedback>();
                if (_feedback == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Feedback for Bullet on " + gameObject);
                }
            }
            _collider.isTrigger = false;
        }

        private void OnEnable()
        {
            _currentBounces = 0;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_hasError) return;
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
                _feedback.BounceFeedback(hit.point, Quaternion.Euler(hit.normal));
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
            if (_hasError) return;
            StartCoroutine(Ignore(objCollider, duration));
        }

        private IEnumerator Ignore(Collider objCollider, float duration)
        {
            Physics.IgnoreCollision(_collider, objCollider, true);
            yield return new WaitForSecondsRealtime(duration);
            Physics.IgnoreCollision(_collider, objCollider, false);
        }

        private void Kill()
        {
            _feedback.DestroyFeedback(transform.position, transform.rotation);
            BulletPool.Instance.ReturnBullet(this);
        }
    }
}