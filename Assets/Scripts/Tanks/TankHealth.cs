using Assets.Scripts.Audio;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Tanks {
    public class TankHealth : MonoBehaviour, IDamageable {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private FloatReference _currentHealth = new FloatReference();

        [Header("Death Feedback")]
        [SerializeField] private AudioClip _deathAudio = null;
        [SerializeField] private ParticleSystem _deathParticles = null;
        [SerializeField] private UnityEvent _onDeath = new UnityEvent();

        public bool Invincible { get; set; }

        private void Awake()
        {
            // Check if mis-assigned current health
            if (!_currentHealth.UseConstant && _currentHealth.Variable == null) {
                _currentHealth.UseConstant = true;
                DebugHelper.Warn(gameObject, "Current Health has no assigned variable");
            }
            // Ensure death particles don't play on awake or self destruct
            if (_deathParticles != null && _deathParticles.gameObject.activeInHierarchy) {
                _deathParticles.gameObject.SetActive(false);
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

        public void OnBulletImpact(int damageTaken)
        {
            DecreaseHealth(damageTaken);
        }

        public void OnKill()
        {
            if (Invincible) return;
            Kill();
        }

        private void Kill()
        {
            _onDeath?.Invoke();
            if (_deathAudio != null) {
                AudioHelper.PlayClip(_deathAudio);
            }
            if (_deathParticles != null) {
                Instantiate(_deathParticles, transform.position, Quaternion.identity).gameObject.SetActive(true);
            }
        }
    }
}