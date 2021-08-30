using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemies {
    public abstract class Enemy : MonoBehaviour {
        [SerializeField] private ParticleSystem _impactParticles = null;
        [SerializeField] private AudioClip _impactSound = null;

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
                Instantiate(_impactParticles, transform.position, Quaternion.identity);
            }
            if (_impactSound != null) {
                AudioHelper.PlayClip2D(_impactSound);
            }
        }
    }
}