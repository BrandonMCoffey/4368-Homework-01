using UnityEngine;

namespace Mechanics.Enemies
{
    public class Bomb : MonoBehaviour
    {
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

        private float _lastTime;
        private bool _flash;
        private bool _hasExploded;

        private void Awake()
        {
            if (_baseMaterial == null && _meshRenderer != null) {
                _baseMaterial = _meshRenderer.material;
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

        private void ToggleFlash()
        {
            _flash = !_flash;
            if (_meshRenderer == null || _flashMaterial == null) return;
            _meshRenderer.material = _flash ? _flashMaterial : _baseMaterial;
        }

        private void Explode()
        {
            if (_visualsToDisable != null) {
                _visualsToDisable.SetActive(false);
            }
            if (_explosionParticles != null) {
                _explosionParticles.Play();
                _lastTime = Time.time;
                _tooFast = _explosionParticles.main.duration;
                _hasExploded = true;
            } else {
                Destroy(gameObject);
            }
        }
    }
}