using System.Collections;
using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    [RequireComponent(typeof(Collider))]
    public abstract class PowerupBase : MonoBehaviour {
        [SerializeField] private float _powerupDuration = 5;
        [SerializeField] private GameObject _art = null;
        [SerializeField] private ParticleSystem _constantParticles = null;
        [SerializeField] private ParticleSystem _collectParticles = null;
        [SerializeField] private AudioClip _powerUpSound = null;
        [SerializeField] private AudioClip _powerDownSound = null;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            TankHealth health = other.gameObject.GetComponent<TankHealth>();
            if (health == null) return;
            StartCoroutine(PowerupCoroutine(health));
        }

        private IEnumerator PowerupCoroutine(TankHealth health)
        {
            _collider.enabled = false;
            if (_art != null) _art.SetActive(false);
            if (_constantParticles != null) {
                ParticleSystem.EmissionModule emission = _constantParticles.emission;
                emission.rateOverTime = 0;
            }
            ActivatePowerup(health);
            Feedback();
            yield return new WaitForSecondsRealtime(_powerupDuration);
            DeactivatePowerup(health);
            gameObject.SetActive(false);
        }

        protected virtual void ActivatePowerup(TankHealth health)
        {
            if (_powerUpSound != null) {
                AudioHelper.PlayClip2D(_powerUpSound);
            }
        }

        protected virtual void DeactivatePowerup(TankHealth health)
        {
            if (_powerDownSound != null) {
                AudioHelper.PlayClip2D(_powerDownSound);
            }
        }

        protected virtual void Feedback()
        {
            if (_collectParticles != null) {
                Instantiate(_collectParticles, transform.position, Quaternion.identity);
            }
        }
    }
}