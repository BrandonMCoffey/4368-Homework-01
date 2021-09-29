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
        [SerializeField] private Color _superChargedColor = Color.cyan;
        [SerializeField] private Color _fullColor = Color.cyan;
        [SerializeField] private Color _emptyColor = Color.red;
        [SerializeField] private Color _deadColor = Color.red;
        [SerializeField] private Material _baseMaterial = null;
        [SerializeField] private List<MeshRenderer> _colorsToChange = new List<MeshRenderer>();
        [Header("Laser")]
        [SerializeField] private GameObject _laserDamageVolume = null;
        [SerializeField] private Transform _laserPrepare = null;
        [SerializeField] private Vector3 _laserPrepareScale = new Vector3(1, 1, 50);
        [SerializeField] private ParticleSystem _laser = null;

        private Material _customMaterial;
        private int _currentHealth;
        private Color _currentColor;
        private bool _invincible;
        private bool _isAttacking;
        private bool _isAlive;

        private void Start()
        {
            _currentHealth = _maxHealth;
            _customMaterial = Instantiate(_baseMaterial);
            foreach (var obj in _colorsToChange) {
                obj.material = _customMaterial;
            }
            _isAlive = true;
            if (_laserDamageVolume != null) {
                _laserDamageVolume.SetActive(false);
            }
        }

        public void StartLaserSequence()
        {
            if (!_isAlive) return;
            _isAttacking = true;
            if (_laserPrepare != null) {
                _laserPrepare.gameObject.SetActive(true);
                _laserPrepare.localScale = Vector3.zero;
            }
        }

        public void Charge(float delta)
        {
            if (!_isAlive) return;
            _customMaterial.color = Color.Lerp(_currentColor, _superChargedColor, delta);
            _laserPrepare.localScale = Vector3.Lerp(Vector3.zero, _laserPrepareScale, delta);
        }

        public void ActivateLaser(float time)
        {
            if (!_isAlive) return;
            _invincible = true;

            if (_laserPrepare != null) {
                _laserPrepare.gameObject.SetActive(false);
            }
            if (_laser != null) {
                ParticleSystem.MainModule main = _laser.main;
                main.duration = time;
                main.loop = false;
                _laser.Play();
            }
            if (_laserDamageVolume != null) {
                _laserDamageVolume.SetActive(true);
            }
        }

        public void DeactivateLaser()
        {
            _invincible = false;
            if (_laserDamageVolume != null) {
                _laserDamageVolume.SetActive(false);
            }
        }

        public void FinishLaserSequence()
        {
            _isAttacking = false;
        }

        public bool OnDamageVolume(int damage)
        {
            return false;
        }

        public void OnTankImpact(int damageTaken)
        {
        }

        public void OnBombDealDamage(int damageTaken)
        {
            if (_invincible || _isAlive) {
                Damage();
            }
        }

        public bool OnBulletImpact(int damageTaken)
        {
            if (_invincible || !_isAlive) return !_bulletsBounceWhenDead;
            Damage();
            return true;
        }

        public void OnKill()
        {
            if (_invincible || !_isAlive) return;
            Kill();
        }

        private void UpdateColor()
        {
            float delta = (float)_currentHealth / _maxHealth;
            _currentColor = Color.Lerp(_emptyColor, _fullColor, delta);
            if (!_isAttacking) {
                _customMaterial.color = _currentColor;
            }
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
            _isAttacking = false;
            _isAlive = false;
            if (_laserPrepare != null) {
                _laserPrepare.gameObject.SetActive(false);
            }
            if (_laser != null) {
                _laser.Stop();
            }
        }

        public void Respawn()
        {
            if (_isAlive) return;
            _customMaterial.color = _emptyColor;
            _isAlive = true;
        }
    }
}