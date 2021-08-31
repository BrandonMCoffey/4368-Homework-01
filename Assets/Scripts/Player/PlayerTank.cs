using System.Collections.Generic;
using Assets.Scripts.Collectibles;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Player {
    [RequireComponent(typeof(TankHealth))]
    [RequireComponent(typeof(Inventory))] // TODO: Implement IInvisible
    public class PlayerTank : MonoBehaviour, IHealable, IInvincible, IInventory<Treasure> {
        [Header("Invincibility Settings")]
        [SerializeField] private Material _invincibilityMaterial = null;
        [SerializeField] private List<MeshRenderer> _materialsToChangeWhenInvincible = new List<MeshRenderer>();
        private List<Material> _regularMaterial;

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

        public void OnSetInvincible()
        {
            _health.Invincible = true;
            _regularMaterial = new List<Material>(_materialsToChangeWhenInvincible.Count);
            foreach (var meshRenderer in _materialsToChangeWhenInvincible) {
                _regularMaterial.Add(meshRenderer.material);
                meshRenderer.material = _invincibilityMaterial;
            }
        }

        public void OnRemoveInvincible()
        {
            _health.Invincible = false;
            for (int i = 0; i < _materialsToChangeWhenInvincible.Count; ++i) {
                _materialsToChangeWhenInvincible[i].material = _regularMaterial[i];
            }
            _regularMaterial.Clear();
        }

        public void OnCollect(Treasure pickup)
        {
            _inventory.AddTreasure(pickup.Value);
        }
    }
}