using System.Collections;
using Audio;
using Game;
using UnityEngine;

namespace Mechanics.Boss
{
    public class BossFeedback : MonoBehaviour
    {
        [SerializeField] private float _screenShakeIntensity = 0.5f;
        [SerializeField] private float _screenShakeDuration = 0.5f;
        [SerializeField] private ParticleSystem _escalationParticles = null;
        [SerializeField] private ParticleSystem _enragedParticles = null;
        [SerializeField] private ParticleSystem _damageAttackParticles = null;
        [SerializeField] private SfxReference _physicalDamageSfx = new SfxReference();
        [SerializeField] private CameraController _cameraController;

        private void Awake()
        {
            if (_cameraController == null) {
                _cameraController = FindObjectOfType<CameraController>();
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
        }
    }
}