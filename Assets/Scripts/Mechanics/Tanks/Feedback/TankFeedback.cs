using System.Collections;
using UnityEngine;

namespace Mechanics.Tanks.Feedback
{
    [RequireComponent(typeof(TankSoundEffects))]
    [RequireComponent(typeof(TankParticles))]
    public class TankFeedback : MonoBehaviour
    {
        [SerializeField] private Transform _visualsTransform = null;
        [SerializeField] private float _visualShakeIntensity = 0.2f;
        [SerializeField] private float _visualShakeDuration = 0.25f;
        [SerializeField] [Range(0, 1)] private float _moveSpeedSmoothing = 0.5f;

        private float _moveSpeed;

        private TankSoundEffects _tankSoundEffects;
        private TankParticles _tankParticles;

        private void Awake()
        {
            _tankSoundEffects = GetComponent<TankSoundEffects>();
            _tankParticles = GetComponent<TankParticles>();
        }

        public void SetMovementSpeed(float speed)
        {
            // Smoothly adjust speed
            _moveSpeed = Mathf.SmoothStep(_moveSpeed, speed, _moveSpeedSmoothing);
            _moveSpeed = Mathf.Clamp(_moveSpeed, 0, 1);

            _tankSoundEffects.SetMoveVolume(_moveSpeed);
            _tankParticles.SetMoveSpeed(_moveSpeed);
        }

        public void SetMovementEffects(int positive, int negative)
        {
            _tankParticles.SetPositiveMoveEffects(positive);
            _tankParticles.SetNegativeMoveEffects(negative);
        }

        public void TurretFireFeedback(Vector3 position, Quaternion rotation)
        {
            _tankSoundEffects.PlayTurretFireSfx(position);
            _tankParticles.PlayTurretFireParticles(position, rotation);
        }

        public void DamageFeedback()
        {
            _tankSoundEffects.PlayDamageSfx();
            _tankParticles.PlayDamageParticles();
            if (_visualsTransform != null) {
                StopAllCoroutines();
                StartCoroutine(ShakePlayer());
            }
        }

        public void DeathFeedback()
        {
            _tankSoundEffects.PlayDeathSfx();
            _tankParticles.PlayDeathParticles();
        }

        private IEnumerator ShakePlayer()
        {
            for (float t = 0; t < _visualShakeDuration; t += Time.deltaTime) {
                float delta = 1 - Mathf.Abs(1f - 2 * t / _visualShakeDuration);
                Vector3 pos = Random.insideUnitSphere * _visualShakeIntensity * delta;
                pos.y = 0;
                _visualsTransform.localPosition = pos;
                yield return null;
            }
            _visualsTransform.localPosition = Vector3.zero;
        }
    }
}