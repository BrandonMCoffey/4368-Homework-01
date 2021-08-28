using System;
using System.Collections.Generic;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankHealth : MonoBehaviour {
        [Header("Health Settings")]
        [SerializeField] private int _maxHealth = 3;
        [SerializeField] private FloatReference _currentHealth = new FloatReference();

        [Header("Invincibility Settings")]
        [SerializeField] private Material _invincibilityMaterial = null;
        [SerializeField] private List<MeshRenderer> _materialsToChangeWhenInvincible = new List<MeshRenderer>();

        private List<Material> _regularMaterial;
        private bool _invincible;
        public bool Invincible => _invincible;

        public Action OnKill = delegate { };

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

        public void DecreaseHealth(int amount)
        {
            if (_invincible) return;
            _currentHealth.Value -= amount;
            if (_currentHealth <= 0) {
                OnKill.Invoke();
            }
        }

        public void SetInvincible()
        {
            _regularMaterial = new List<Material>(_materialsToChangeWhenInvincible.Count);
            foreach (var meshRenderer in _materialsToChangeWhenInvincible) {
                _regularMaterial.Add(meshRenderer.material);
                meshRenderer.material = _invincibilityMaterial;
            }
            _invincible = true;
        }

        public void RemoveInvincible()
        {
            if (!_invincible) return;
            for (int i = 0; i < _materialsToChangeWhenInvincible.Count; ++i) {
                _materialsToChangeWhenInvincible[i].material = _regularMaterial[i];
            }
            _invincible = false;
            _regularMaterial.Clear();
        }
    }
}