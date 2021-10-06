using System.Collections;
using System.Collections.Generic;
using Audio;
using Game;
using UnityEngine;

namespace Mechanics.Boss
{
    public class BossFeedback : MonoBehaviour
    {
        [Header("Screen Shake")]
        [SerializeField] private float _screenShakeIntensity = 0.5f;
        [SerializeField] private float _screenShakeDuration = 0.5f;
        [Header("Visuals")]
        [SerializeField] private float _damagedTimer = 0.25f;
        [SerializeField] private List<MeshRenderer> _materialsToSet = new List<MeshRenderer>();
        [SerializeField] private Material _baseMaterial = null;
        [SerializeField] private Material _damagedMaterial = null;
        [SerializeField] private Material _deadMaterial = null;
        [Header("Particles")]
        [SerializeField] private ParticleSystem _escalationParticles = null;
        [SerializeField] private ParticleSystem _enragedParticles = null;
        [SerializeField] private ParticleSystem _damageAttackParticles = null;
        [Header("References")]
        [SerializeField] private SfxReference _physicalDamageSfx = new SfxReference();
        [SerializeField] private CameraController _cameraController;

        private bool _isDead;

        private void Awake()
        {
            if (_cameraController == null) {
                _cameraController = FindObjectOfType<CameraController>();
            }
        }

        public void OnDamaged()
        {
            if (_isDead) return;
            StopAllCoroutines();
            StartCoroutine(Damaged());
        }

        public IEnumerator Damaged()
        {
            foreach (var mat in _materialsToSet) {
                mat.material = _damagedMaterial;
            }
            yield return new WaitForSecondsRealtime(_damagedTimer);
            if (_isDead) yield break;

            foreach (var mat in _materialsToSet) {
                mat.material = _baseMaterial;
            }
        }

        public void DamageAttack(Vector3 forward)
        {
            _physicalDamageSfx.Play();
            if (_damageAttackParticles == null) return;
            _damageAttackParticles.transform.forward = forward;
            _damageAttackParticles.Play();
        }

        public void ShakeScreen()
        {
            _cameraController.ShakeCamera(_screenShakeDuration, _screenShakeIntensity);
        }

        public void EscalationFeedback()
        {
            _cameraController.ShakeCamera(_screenShakeDuration * 2, _screenShakeIntensity * 0.75f);
            if (_escalationParticles != null) {
                _escalationParticles.Play();
            }
        }

        public void MidpointFeedback()
        {
            if (_enragedParticles != null) {
                _enragedParticles.Play();
            }
        }

        public void KillSequenceFeedback()
        {
            _isDead = true;
            foreach (var mat in _materialsToSet) {
                mat.material = _deadMaterial;
            }
        }
    }
}