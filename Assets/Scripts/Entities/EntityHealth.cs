using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities {
    public class EntityHealth : MonoBehaviour {
        [SerializeField] private int _maxHealth = 3;
        private int _currentHealth;
        private bool _invincible;
        public bool Invincible => _invincible;

        public Action OnKill = delegate { };

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void IncreaseHealth(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        }

        public void DecreaseHealth(int amount)
        {
            if (_invincible) return;
            _currentHealth -= amount;
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