using Assets.Scripts.Audio;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemies.Basic {
    public abstract class Enemy : MonoBehaviour {
        [SerializeField] private ParticleSystem _impactParticles = null;
        [SerializeField] private AudioClip _impactSound = null;

        protected virtual void Awake()
        {
            // Ensure impact particles don't play on awake or self destruct
            if (_impactParticles != null && _impactParticles.gameObject.activeInHierarchy) {
                _impactParticles.gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (OnImpact(other.gameObject)) {
                ImpactFeedback();
            }
        }

        protected abstract bool OnImpact(GameObject other);

        protected void ImpactFeedback()
        {
            if (_impactParticles != null) {
                Instantiate(_impactParticles, transform.position, Quaternion.identity).gameObject.SetActive(true);
            }
            if (_impactSound != null) {
                AudioHelper.PlayClip(_impactSound);
            }
        }
    }
}