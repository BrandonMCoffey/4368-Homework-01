using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Tanks
{
    public class TankParticleController : MonoBehaviour
    {
        [SerializeField] private TankMovement _tankMovement = null;
        [SerializeField] private ParticleSystem _speedBoostEffects = null;
        [SerializeField] private ParticleSystem _slowedDownEffects = null;
        [SerializeField] private float _particleAmount = 10;

        private void Awake()
        {
            if (_tankMovement == null) DebugHelper.Warn(gameObject, "No Tank Movement Connected");
        }

        private void Update()
        {
            if (_tankMovement == null) return;
            SpeedBoostFeedback();
            SlowedDownFeedback();
        }

        private void SpeedBoostFeedback()
        {
            if (_speedBoostEffects == null) return;
            ParticleSystem.EmissionModule emission = _speedBoostEffects.emission;
            emission.rateOverTime = _tankMovement.MoveSpeed.ActivePositiveEffects * _particleAmount;
        }

        private void SlowedDownFeedback()
        {
            if (_slowedDownEffects == null) return;
            ParticleSystem.EmissionModule emission = _slowedDownEffects.emission;
            emission.rateOverTime = _tankMovement.MoveSpeed.ActiveNegativeEffects * _particleAmount;
        }
    }
}