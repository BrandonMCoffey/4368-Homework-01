using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Projectiles
{
    public class BulletFeedback : MonoBehaviour
    {
        [Header("Bounce Feedback")]
        [SerializeField] private SfxReference _bounceSfx = new SfxReference();
        [SerializeField] private ParticleSystem _bounceParticles = null;
        [Header("Destroy Feedback")]
        [SerializeField] private SfxReference _destroySfx = new SfxReference();
        [SerializeField] private ParticleSystem _destroyParticles = null;

        private void Awake()
        {
            // Ensure collect particles don't play on awake or self destruct
            if (_bounceParticles != null && _bounceParticles.gameObject.activeInHierarchy) {
                var main = _bounceParticles.main;
                main.playOnAwake = false;
                _bounceParticles.Stop();
            }
            if (_destroyParticles != null && _destroyParticles.gameObject.activeInHierarchy) {
                _destroyParticles.gameObject.SetActive(false);
            }
        }

        public void BounceFeedback(Vector3 position, Quaternion rotation)
        {
            if (_bounceParticles != null) {
                _bounceParticles.gameObject.SetActive(true);
                _bounceParticles.transform.SetPositionAndRotation(position, rotation);
                _bounceParticles.Play();
            }
            _bounceSfx.Play();
        }

        public void DestroyFeedback(Vector3 position, Quaternion rotation)
        {
            if (_destroyParticles != null) {
                Instantiate(_destroyParticles, position, rotation).gameObject.SetActive(true);
            }
            _destroySfx.Play();
        }
    }
}