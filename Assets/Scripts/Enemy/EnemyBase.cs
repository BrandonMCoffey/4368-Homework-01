using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBase : MonoBehaviour {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private ParticleSystem _impactParticles;
        [SerializeField] private AudioClip _impactSound = null;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnCollisionEnter(Collision other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player == null) return;
            PlayerImpact(player);
            ImpactFeedback();
        }

        protected virtual void Move()
        {
        }

        protected virtual void PlayerImpact(Player player)
        {
            player.Health.DecreaseHealth(_damageAmount);
        }

        protected void ImpactFeedback()
        {
            if (_impactParticles != null) {
                _impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.identity);
            }
            if (_impactSound != null) {
                AudioHelper.PlayClip2D(_impactSound);
            }
        }
    }
}