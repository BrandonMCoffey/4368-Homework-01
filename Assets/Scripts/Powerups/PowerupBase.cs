using System.Collections;
using Assets.Scripts.Audio;
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

        protected float Duration => _powerupDuration;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            // Ensure collect particles don't play on awake or self destruct
            if (_collectParticles != null && _collectParticles.gameObject.activeInHierarchy) {
                _collectParticles.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (OnCollect(other.gameObject)) {
                StartCoroutine(PowerupCoroutine());
            }
        }

        protected abstract bool OnCollect(GameObject other);

        private IEnumerator PowerupCoroutine()
        {
            HideObject();
            Activate();
            ActivationFeedback();
            yield return new WaitForSecondsRealtime(_powerupDuration);
            Deactivate();
            DeactivationFeedback();
            DisableObject();
        }

        protected abstract void Activate();
        protected abstract void Deactivate();

        protected virtual void HideObject()
        {
            _collider.enabled = false;
            if (_art != null) _art.SetActive(false);
            if (_constantParticles != null) {
                ParticleSystem.EmissionModule emission = _constantParticles.emission;
                emission.rateOverTime = 0;
            }
        }

        protected virtual void ActivationFeedback()
        {
            if (_collectParticles != null) {
                Instantiate(_collectParticles, transform.position, Quaternion.identity).gameObject.SetActive(true);
            }
            if (_powerUpSound != null) {
                AudioHelper.PlayClip(_powerUpSound);
            }
        }

        protected virtual void DeactivationFeedback()
        {
            if (_powerDownSound != null) {
                AudioHelper.PlayClip(_powerDownSound);
            }
        }

        protected virtual void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}