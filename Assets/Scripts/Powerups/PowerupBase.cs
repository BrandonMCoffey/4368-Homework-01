using System.Collections;
using Assets.Scripts.Entities;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    [RequireComponent(typeof(Collider))]
    public abstract class PowerupBase : MonoBehaviour {
        [SerializeField] private float _powerupDuration = 5;
        [SerializeField] private GameObject _art = null;
        [SerializeField] private ParticleSystem _collectParticles;
        [SerializeField] private AudioClip _collectSound = null;

        private void OnTriggerEnter(Collider other)
        {
            EntityHealth health = other.gameObject.GetComponent<EntityHealth>();
            if (health == null) return;
            StartCoroutine(PowerupCoroutine(health));
        }

        private IEnumerator PowerupCoroutine(EntityHealth health)
        {
            _art.SetActive(false);
            ActivatePowerup(health);
            Feedback();
            yield return new WaitForSecondsRealtime(_powerupDuration);
            DeactivatePowerup(health);
            gameObject.SetActive(false);
        }

        protected abstract void ActivatePowerup(EntityHealth health);

        protected abstract void DeactivatePowerup(EntityHealth health);

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
    }
}