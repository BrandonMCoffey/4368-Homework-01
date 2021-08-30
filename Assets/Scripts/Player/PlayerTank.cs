using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Player {
    [RequireComponent(typeof(TankHealth))]
    [RequireComponent(typeof(Inventory))]
    public class PlayerTank : MonoBehaviour {
        //[SerializeField] private ParticleSystem _slowedDownEffects = null;
        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }
        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            Health = GetComponent<TankHealth>();
            Movement = GetComponent<TankMovement>();
            Inventory = GetComponent<Inventory>();

            if (Movement == null) {
                DebugHelper.Error(gameObject, "No assigned movement");
            }
        }

        private void Update()
        {
            //if (_slowedDownEffects == null) return;
            //ParticleSystem.MainModule main = _slowedDownEffects.main;
            //ParticleSystem.EmissionModule emission = _slowedDownEffects.emission;
            //main.startSpeedMultiplier = Movement.Speed * _speedMultiplier;
            //emission.rateOverTime = AdjustMoveSpeed.ActiveEffects * _particleMultiplier;
        }
    }
}