using System;
using System.Collections;
using Assets.Scripts.Utility.FloatRef;
using UnityEngine;

namespace Assets.Scripts.Entities {
    public class EntityHealth : MonoBehaviour {
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private FloatReference _currentHealth = new FloatReference();
        private bool _invincible;
        public bool Invincible => _invincible;

        public Action OnKill = delegate { };

        private void Start()
        {
            _currentHealth.Value = _maxHealth;
        }

        public void IncreaseHealth(int amount)
        {
            _currentHealth.Value = _currentHealth + amount;
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value, 0, _maxHealth);
        }

        public void DecreaseHealth(int amount)
        {
            if (_invincible) return;
            _currentHealth.Value -= amount;
            if (_currentHealth <= 0) {
                OnKill.Invoke();
            }
        }

        public void SetInvincible(float duration)
        {
            StartCoroutine(Invincibility(duration));
        }

        private IEnumerator Invincibility(float duration)
        {
            _invincible = true;
            yield return new WaitForSecondsRealtime(duration);
            _invincible = false;
        }
    }
}