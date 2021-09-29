using Audio;
using UnityEngine;

namespace Mechanics.Tanks.Feedback
{
    public class TankSoundEffects : MonoBehaviour
    {
        [SerializeField] private SfxReference _movementSfx = new SfxReference();
        [SerializeField] private SfxReference _turretFireSfx = new SfxReference();
        [SerializeField] private SfxReference _damageSfx = new SfxReference();
        [SerializeField] private SfxReference _deathSfx = new SfxReference();

        private AudioSourceController _movementSfxController;

        private void Start()
        {
            if (!_movementSfx.NullTest()) {
                _movementSfxController = AudioManager.Instance.GetController();
                _movementSfx.Play(_movementSfxController);
            }
            SetMoveVolume(0);
        }

        public void SetMoveVolume(float volume)
        {
            if (_movementSfxController == null) return;
            _movementSfxController.SetCustomVolume(volume);
        }

        public void PlayTurretFireSfx(Vector3 position)
        {
            _turretFireSfx?.PlayAtPosition(position);
        }

        public void PlayDamageSfx()
        {
            _damageSfx?.Play();
        }

        public void PlayDeathSfx()
        {
            _deathSfx?.Play();
        }
    }
}