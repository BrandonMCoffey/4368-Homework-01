using System.Collections;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    [RequireComponent(typeof(EntityHealth))]
    [RequireComponent(typeof(Inventory))]
    public class Player : Entity {
        private TankController _tankController;
        private Inventory _inventory;

        protected override void Awake()
        {
            base.Awake();
            _tankController = GetComponent<TankController>();
            _inventory = GetComponent<Inventory>();
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