using Assets.Scripts.Collectibles;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Player {
    [RequireComponent(typeof(TankHealth), typeof(Inventory), typeof(TankPowerup))]
    public class PlayerTank : MonoBehaviour, IHealable, IInventory<Treasure> {
        private TankHealth _health;
        private TankMovement _movement;
        private Inventory _inventory;

        private void Awake()
        {
            _health = GetComponent<TankHealth>();
            _movement = GetComponent<TankMovement>();
            _inventory = GetComponent<Inventory>();

            if (_movement == null) DebugHelper.Error(gameObject, "No assigned Tank Movement");
        }

        public bool OnHeal(int amount)
        {
            return _health.IncreaseHealth(amount);
        }

        public void OnCollect(Treasure pickup)
        {
            _inventory.AddTreasure(pickup.Value);
        }
    }
}