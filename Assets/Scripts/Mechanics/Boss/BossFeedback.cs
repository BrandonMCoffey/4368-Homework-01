using System.Collections;
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
        [SerializeField] private CameraController _cameraController;

        private void Awake()
        {
            if (_cameraController == null) {
                _cameraController = FindObjectOfType<CameraController>();
            }
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