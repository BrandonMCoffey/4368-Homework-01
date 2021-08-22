using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public abstract class CollectibleBase : MonoBehaviour {
        protected abstract void Collect(Player player);

        [SerializeField] private float _movementSpeed = 1;
        [SerializeField] private ParticleSystem _collectParticles;
        [SerializeField] private AudioClip _collectSound;

        protected float MovementSpeed => _movementSpeed;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Movement(_rb);
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player == null) return;
            Collect(player);
            Feedback();
            DisableObject();
        }

        protected virtual void Movement(Rigidbody rb)
        {
            Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
            rb.MoveRotation(_rb.rotation * turnOffset);
        }

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