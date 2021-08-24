using Assets.Scripts.Entities;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    [RequireComponent(typeof(Collider))]
    public abstract class PowerupBase : MonoBehaviour {
        [SerializeField] private float _powerupDuration = 5;
        [SerializeField] private ParticleSystem _collectParticles;
        [SerializeField] private AudioClip _collectSound = null;
        protected float PowerupDuration => _powerupDuration;

        private void OnTriggerEnter(Collider other)
        {
            EntityHealth health = other.gameObject.GetComponent<EntityHealth>();
            if (health == null) return;
            ActivatePowerup(health);
            Feedback();
            DisableObject();
        }

        protected abstract void ActivatePowerup(EntityHealth health);

        protected virtual void Feedback()
        {
            if (_collectParticles != null) {
                _collectParticles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
            }
            // Audio (TODO: Consider Object Pooling for performance)
            if (_collectSound != null) {
                AudioHelper.PlayClip2D(_collectSound);
            }
        }

        protected virtual void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}