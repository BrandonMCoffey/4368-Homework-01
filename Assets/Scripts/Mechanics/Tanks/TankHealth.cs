using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Mechanics.Tanks.Feedback;
using Mechanics.Tanks.Movement;
using UnityEngine;
using Utility.CustomFloats;
using Utility.GameEvents.Logic;

namespace Mechanics.Tanks
{
    public class TankHealth : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private FloatReference _currentHealth = new FloatReference();

        [Header("Death Feedback")]
        [SerializeField] private TankFeedback _deathFeedback = null;
        [SerializeField] private List<GameObject> _objsToDisable = new List<GameObject>();
        [SerializeField] private GameEvent _onDamaged = null;
        [SerializeField] private GameEvent _onDeath = null;
        [SerializeField] private bool _destroyOnDeath = false;

        public bool Invincible { get; set; }
        public float Health => _currentHealth;
        public int MaxHealth => _maxHealth;

        private TankMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<TankMovement>();
            if (_movement == null) {
                _movement = GetComponentInChildren<TankMovement>();
            }

            // Check if mis-assigned current health
            if (!_currentHealth.UseConstant && _currentHealth.Variable == null) {
                _currentHealth.UseConstant = true;
            }
            _currentHealth.Value = _maxHealth;
        }

        private void Start()
        {
            _currentHealth.Value = _maxHealth;
        }

        public bool IncreaseHealth(int amount)
        {
            if (_currentHealth >= _maxHealth) return false;
            _currentHealth.Value = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
            OnHealthChanged();
            return true;
        }

        protected virtual bool DecreaseHealth(int amount)
        {
            if (Invincible) return false;
            _currentHealth.Value -= amount;
            if (_onDamaged != null) {
                _onDamaged.Invoke();
            }
            if (_deathFeedback != null) {
                _deathFeedback.DamageFeedback();
            }
            if (_currentHealth <= 0) {
                OnKill();
                return true;
            }
            OnHealthChanged();
            return false;
        }

        protected virtual void OnHealthChanged()
        {
        }

        public bool OnDamageVolume(int damageTaken)
        {
            return DecreaseHealth(damageTaken);
        }

        public void OnTankImpact(int damageTaken)
        {
            DecreaseHealth(damageTaken);
        }

        public void OnBombDealDamage(int damageTaken)
        {
            DecreaseHealth(damageTaken);
        }

        public bool OnBulletImpact(int damageTaken, Vector3 forward)
        {
            DecreaseHealth(damageTaken);
            if (_movement != null) {
                _movement.SetForce(forward);
            }
            return true;
        }

        public void SetHealth(float percent)
        {
            percent = Mathf.Clamp01(percent);
            _currentHealth.Value = MaxHealth * percent;
        }

        public void OnKill()
        {
            if (Invincible) return;
            Kill();
        }

        private void Kill()
        {
            _currentHealth.Value = 0;
            foreach (var obj in _objsToDisable.Where(obj => obj != null)) {
                obj.SetActive(false);
            }
            Collider c = GetComponent<Collider>();
            if (c != null) {
                c.enabled = false;
            }
            Rigidbody r = GetComponent<Rigidbody>();
            if (r != null) {
                r.useGravity = false;
                r.velocity = Vector3.zero;
            }
            if (_deathFeedback != null) {
                _deathFeedback.DeathFeedback();
                _deathFeedback.SetMovementSpeed(0);
            }
            if (_onDeath != null) {
                _onDeath.Invoke();
            }
            if (_destroyOnDeath) {
                StartCoroutine(DestroySelf());
            }
        }

        private IEnumerator DestroySelf()
        {
            yield return new WaitForSecondsRealtime(1f);
            Destroy(gameObject);
        }
    }
}