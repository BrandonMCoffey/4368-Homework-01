using UnityEngine;

namespace Assets.Scripts.Tanks.Feedback
{
    [RequireComponent(typeof(TankSoundEffects))]
    [RequireComponent(typeof(TankParticles))]
    public class TankFeedback : MonoBehaviour
    {
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

        public void DeathFeedback()
        {
            _tankSoundEffects.PlayDeathSfx();
            _tankParticles.PlayDeathParticles();
        }
    }
}