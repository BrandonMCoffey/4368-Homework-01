using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Mechanics.Tanks.Feedback;
using UnityEngine;
using Utility.CustomFloats;

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

        public bool Invincible { get; set; }

        private void Awake()
        {
            // Check if mis-assigned current health
            if (!_currentHealth.UseConstant && _currentHealth.Variable == null) {
                _currentHealth.UseConstant = true;
            }
        }

        private void Start()
        {
            _currentHealth.Value = _maxHealth;
        }

        public bool IncreaseHealth(int amount)
        {
            if (_currentHealth >= _maxHealth) return false;
            _currentHealth.Value = _currentHealth + amount;
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value, 0, _maxHealth);
            return true;
        }

        private void DecreaseHealth(int amount)
        {
            if (Invincible) return;
            _currentHealth.Value -= amount;
            if (_currentHealth <= 0) {
                OnKill();
            }
        }

        public void OnTankImpact(int damageTaken)
        {
            DecreaseHealth(damageTaken);
        }

        public bool OnBulletImpact(int damageTaken)
        {
            DecreaseHealth(damageTaken);
            return true;
        }

        public void OnKill()
        {
            if (Invincible) return;
            Kill();
        }

        private void Kill()
        {
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
            }
        }
    }
}