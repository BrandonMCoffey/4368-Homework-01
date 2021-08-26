using System.Collections;
using Assets.Scripts.Entities;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    [RequireComponent(typeof(EntityHealth))]
    [RequireComponent(typeof(Inventory))]
    public class Player : Entity {
        [Header("Audio")]
        [SerializeField] private AudioClip _deathSound = null;

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
            if (_deathSound != null) {
                AudioHelper.PlayClip2D(_deathSound);
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