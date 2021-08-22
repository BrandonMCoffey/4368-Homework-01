using System;
using UnityEngine;

namespace Assets.Scripts {
    public class Health : MonoBehaviour {
        [SerializeField] private int _maxHealth = 3;
        private int _currentHealth;

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
            _currentHealth -= amount;
            if (_currentHealth <= 0) {
                OnKill.Invoke();
            }
        }
    }
}