using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Level_Systems
{
    public class EnergyCell : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private bool _bulletsBounceWhenDead = true;
        [Header("Colors and Materials")]
        [SerializeField] private Color _fullColor = Color.cyan;
        [SerializeField] private Color _emptyColor = Color.red;
        [SerializeField] private Color _deadColor = Color.red;
        [SerializeField] private Material _baseMaterial = null;
        [SerializeField] private List<MeshRenderer> _colorsToChange = new List<MeshRenderer>();

        private Material _customMaterial;
        private int _currentHealth;
        private bool _isAlive;

        private void Start()
        {
            _currentHealth = _maxHealth;
            _customMaterial = Instantiate(_baseMaterial);
            foreach (var obj in _colorsToChange) {
                obj.material = _customMaterial;
            }
            _isAlive = true;
        }

        public void OnTankImpact(int damageTaken)
        {
        }

        public bool OnBulletImpact(int damageTaken)
        {
            if (!_isAlive) return !_bulletsBounceWhenDead;
            Damage();
            return true;
        }

        public void OnKill()
        {
            if (!_isAlive) return;
            Kill();
        }

        private void UpdateColor()
        {
            float delta = (float)_currentHealth / _maxHealth;
            _customMaterial.color = Color.Lerp(_emptyColor, _fullColor, delta);
        }

        private void Damage()
        {
            _currentHealth--;
            UpdateColor();
            if (_currentHealth < 0) {
                OnKill();
            }
        }

        private void Kill()
        {
            _customMaterial.color = _deadColor;
            _isAlive = false;
        }
    }
}