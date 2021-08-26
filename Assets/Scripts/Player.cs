using Assets.Scripts.Entities;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.GameEvents.Logic;
using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    [RequireComponent(typeof(EntityHealth))]
    [RequireComponent(typeof(Inventory))]
    public class Player : Entity {
        [SerializeField] private GameEvent _onDeath = null;

        private TankController _tankController;
        private Inventory _inventory;

        protected override void Awake()
        {
            base.Awake();
            _tankController = GetComponent<TankController>();
            _inventory = GetComponent<Inventory>();
        }

        public override void Kill()
        {
            if (Health != null && Health.Invincible) return;
            if (_onDeath != null) {
                _onDeath.Invoke();
            }
            base.Kill();
        }

        public TankController GetTankController()
        {
            return _tankController;
        }

        public Inventory GetInventory()
        {
            return _inventory;
        }
    }
}