using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.GameEvents.Logic;
using UnityEngine;

namespace Assets.Scripts.Player {
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(TankHealth))]
    [RequireComponent(typeof(TankMovement))]
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerTank : MonoBehaviour {
        [SerializeField] private GameEvent _onDeath = null;
        [SerializeField] private AudioClip _deathAudio = null;
        [SerializeField] private ParticleSystem _deathParticles = null;

        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }
        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            Health = GetComponent<TankHealth>();
            Movement = GetComponent<TankMovement>();
            Inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            Health.OnKill += Kill;
        }

        private void OnDisable()
        {
            Health.OnKill -= Kill;
        }

        public void Kill()
        {
            if (Health.Invincible) return;
            if (_onDeath != null) {
                _onDeath.Invoke();
            }
            if (_deathAudio != null) {
                AudioHelper.PlayClip2D(_deathAudio);
            }
            if (_deathParticles != null) {
                Instantiate(_deathParticles, transform.position, Quaternion.identity);
            }
        }
    }
}