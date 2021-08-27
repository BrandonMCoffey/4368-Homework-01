using Assets.Scripts.Entities;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBase : Entity {
        [SerializeField] private int _damageAmount = 1;
        [SerializeField] private ParticleSystem _impactParticles = null;
        [SerializeField] private AudioClip _impactSound = null;
        protected ParticleSystem ImpactParticles => _impactParticles;

        private Rigidbody _rb;

        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player == null) return;
            PlayerImpact(player);
            ImpactFeedback();
        }

        protected virtual void PlayerImpact(Player player)
        {
            player.Health.DecreaseHealth(_damageAmount);
        }

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