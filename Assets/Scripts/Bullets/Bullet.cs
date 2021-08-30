using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.Bullets {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Bullet : MonoBehaviour {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _lifeSpan = 10f;
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private int _bounceTimes = 1;

        private Rigidbody _rb;
        private Collider _collider;
        private Coroutine _deathCoroutine;
        private int _currentBounces;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnEnable()
        {
            _deathCoroutine = StartCoroutine(AutoKill());
        }

        private void OnDisable()
        {
            if (_deathCoroutine != null) {
                StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerTank>()) return;
            TankHealth health = other.gameObject.GetComponent<TankHealth>();
            if (health != null) {
                health.DecreaseHealth(_damageAmount);
            } else if (_currentBounces++ < _bounceTimes) {
                Kill();
                //Vector3 contact = other.GetContact(0).normal;
                //float dot = Vector3.Dot(contact, -transform.forward) * 2;
                //Vector3 reflection = contact * dot + transform.forward;
                //_rb.velocity = transform.TransformDirection(reflection.normalized * 15f);
            } else {
                Kill();
            }
        }

        private void Move()
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }

        private IEnumerator AutoKill()
        {
            yield return new WaitForSecondsRealtime(_lifeSpan);
            Kill();
        }

        private void Kill()
        {
            if (_deathCoroutine != null) {
                StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }
            Destroy(gameObject);
        }
    }
}