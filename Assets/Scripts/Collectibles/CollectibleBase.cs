using Assets.Scripts.PlayerTank;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class CollectibleBase : MonoBehaviour {
        [SerializeField] private float _movementSpeed = 1;
        [SerializeField] private ParticleSystem _collectParticles = null;
        [SerializeField] private AudioClip _collectSound = null;

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
            bool wasCollected = Collect(player);
            if (!wasCollected) return;
            Feedback();
            DisableObject();
        }

        protected abstract bool Collect(Player player);

        protected virtual void Feedback()
        {
            if (_collectParticles != null) {
                Instantiate(_collectParticles, transform.position, Quaternion.identity);
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
            Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
            rb.MoveRotation(_rb.rotation * turnOffset);
        }
    }
}