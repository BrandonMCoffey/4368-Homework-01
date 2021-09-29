using Audio;
using Interfaces;
using Mechanics.Projectiles;
using UnityEngine;

namespace Mechanics.Enemies
{
    public class Bomb : MonoBehaviour
    {
        [Header("Bomb settings")]
        [SerializeField] private float _range = 1;
        [SerializeField] private int _damage = 1;
        [Header("Timer Settings")]
        [SerializeField] private float _speed = 5;
        [SerializeField] private float _accel = 0.2f;
        [SerializeField] private float _tooFast = 1;
        [Header("Flash Settings")]
        [SerializeField] private MeshRenderer _meshRenderer = null;
        [SerializeField] private Material _baseMaterial;
        [SerializeField] private Material _flashMaterial = null;
        [Header("References")]
        [SerializeField] private GameObject _visualsToDisable = null;
        [SerializeField] private ParticleSystem _explosionParticles = null;
        [SerializeField] private SfxReference _explosionSfx = new SfxReference();

        private float _lastTime;
        private bool _flash;
        private bool _hasExploded;
        private Collider _collider;

        private void Awake()
        {
            if (_baseMaterial == null && _meshRenderer != null) {
                _baseMaterial = _meshRenderer.material;
            }
            _collider = GetComponent<Collider>();
            if (_collider != null) {
                _collider.isTrigger = true;
            }
        }

        private void OnEnable()
        {
            _lastTime = Time.time;
        }

        private void Update()
        {
            if (_hasExploded) {
                if (Time.time - _lastTime > _tooFast) {
                    Destroy(gameObject);
                }
                return;
            }
            if (Time.time - _lastTime > _speed) {
                _lastTime = Time.time;
                ToggleFlash();
                _speed -= _accel;
                if (_speed <= _tooFast) {
                    Explode();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet == null) return;
            bullet.Kill();
            Explode();
        }

        private void ToggleFlash()
        {
            _flash = !_flash;
            if (_meshRenderer == null || _flashMaterial == null) return;
            _meshRenderer.material = _flash ? _flashMaterial : _baseMaterial;
        }

        private void Explode()
        {
            if (_collider != null) {
                _collider.enabled = false;
            }
            if (_visualsToDisable != null) {
                _visualsToDisable.SetActive(false);
            }
            _explosionSfx.Play();
            DealDamage();
            if (_explosionParticles != null) {
                _explosionParticles.Play();
                _lastTime = Time.time;
                _tooFast = _explosionParticles.main.duration;
                _hasExploded = true;
            } else {
                Destroy(gameObject);
            }
        }

        private void DealDamage()
        {
            var colliders = Physics.OverlapSphere(transform.position, _range);
            foreach (var col in colliders) {
                IDamageable damageableObj = col.GetComponent<IDamageable>();
                damageableObj?.OnBombDealDamage(_damage);
            }
        }
    }
}