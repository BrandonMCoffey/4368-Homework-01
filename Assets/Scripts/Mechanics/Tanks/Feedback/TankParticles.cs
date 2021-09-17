using UnityEngine;

namespace Assets.Scripts.Mechanics.Tanks.Feedback
{
    public class TankParticles : MonoBehaviour
    {
        [Header("Movement Particles")]
        [SerializeField] private ParticleSystem _movementParticles = null;
        [SerializeField] private ParticleSystem _speedBoostEffects = null;
        [SerializeField] private ParticleSystem _slowedDownEffects = null;
        [Header("Other Particles")]
        [SerializeField] private ParticleSystem _turretFireParticles = null;
        [SerializeField] private ParticleSystem _deathParticles = null;

        private float _movementParticleAmount;
        private float _speedBoostParticleAmount;
        private float _slowedDownParticleAmount;

        private void Awake()
        {
            if (_movementParticles != null) {
                _movementParticles.gameObject.SetActive(true);
                _movementParticleAmount = _movementParticles.emission.rateOverTime.constant;
            }
            if (_speedBoostEffects != null) {
                _speedBoostEffects.gameObject.SetActive(true);
                _speedBoostParticleAmount = _speedBoostEffects.emission.rateOverTime.constant;
            }
            if (_slowedDownEffects != null) {
                _slowedDownEffects.gameObject.SetActive(true);
                _slowedDownParticleAmount = _slowedDownEffects.emission.rateOverTime.constant;
            }
        }

        private void Start()
        {
            SetMoveSpeed(0);
            SetPositiveMoveEffects(0);
            SetNegativeMoveEffects(0);
        }

        public void SetMoveSpeed(float amount)
        {
            if (_movementParticles == null) return;
            ParticleSystem.EmissionModule emission = _movementParticles.emission;
            emission.rateOverTime = amount * _movementParticleAmount;
        }

        public void SetPositiveMoveEffects(int amount)
        {
            if (_speedBoostEffects == null) return;
            ParticleSystem.EmissionModule emission = _speedBoostEffects.emission;
            emission.rateOverTime = amount * _speedBoostParticleAmount;
        }

        public void SetNegativeMoveEffects(int amount)
        {
            if (_slowedDownEffects == null) return;
            ParticleSystem.EmissionModule emission = _slowedDownEffects.emission;
            emission.rateOverTime = amount * _slowedDownParticleAmount;
        }

        public void PlayTurretFireParticles(Vector3 position, Quaternion rotation)
        {
            _turretFireParticles.transform.SetPositionAndRotation(position, rotation);
            _turretFireParticles.Play();
        }

        public void PlayDeathParticles()
        {
            _deathParticles.Play();
        }
    }
}