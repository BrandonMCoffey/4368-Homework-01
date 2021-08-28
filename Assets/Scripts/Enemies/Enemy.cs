using Assets.Scripts.Player;
using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemies {
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : Tank {
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
            PlayerTank playerTank = other.gameObject.GetComponent<PlayerTank>();
            if (playerTank == null) return;
            PlayerImpact(playerTank);
            ImpactFeedback();
        }

        protected virtual void PlayerImpact(PlayerTank playerTank)
        {
            playerTank.Health.DecreaseHealth(_damageAmount);
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