using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class CollectibleBase : MonoBehaviour {
        [Header("Feedback")]
        [SerializeField] private ParticleSystem _collectParticles = null;
        [SerializeField] private AudioClip _collectSound = null;
        [Header("Movement")]
        [SerializeField] private float _rotationSpeed = 1;

        protected float RotationSpeed => _rotationSpeed;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            GetComponent<Collider>().isTrigger = true;
            // Ensure collect particles don't play on awake or self destruct
            if (_collectParticles != null && _collectParticles.gameObject.activeInHierarchy) {
                _collectParticles.gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            Movement(_rb);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (OnCollect(other.gameObject)) {
                Feedback();
                DisableObject();
            }
        }

        protected abstract bool OnCollect(GameObject other);

        protected virtual void Feedback()
        {
            if (_collectParticles != null) {
                Instantiate(_collectParticles, transform.position, Quaternion.identity).gameObject.SetActive(true);
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

        protected virtual void Movement(Rigidbody rb)
        {
            Quaternion turnOffset = Quaternion.Euler(0, _rotationSpeed, 0);
            rb.MoveRotation(_rb.rotation * turnOffset);
        }
    }
}