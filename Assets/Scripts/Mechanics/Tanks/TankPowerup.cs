using System.Collections.Generic;
using Interfaces;
using Mechanics.Tanks.Movement;
using UnityEngine;
using Utility.CustomFloats;

namespace Mechanics.Tanks
{
    public class TankPowerup : MonoBehaviour, IInvincible, IMoveable, IIncreaseable
    {
        [Header("References")]
        [SerializeField] private TankMovement _movement;
        [SerializeField] private TankFire _fire;

        [Header("Invincibility Settings")]
        [SerializeField] private Material _invincibilityMaterial = null;
        [SerializeField] private List<MeshRenderer> _materialsToChangeWhenInvincible = new List<MeshRenderer>();
        private List<Material> _regularMaterial;

        [Header("Invisibility Settings")]
        [SerializeField] private List<GameObject> _regularArt = new List<GameObject>();
        [SerializeField] private List<GameObject> _invisibleArt = new List<GameObject>();

        private TankHealth _health;

        private void Awake()
        {
            _health = GetComponent<TankHealth>();
        }

        private void Start()
        {
            if (_movement == null) {
                _movement = GetComponentInChildren<TankMovement>();
            }
        }

        public void OnSetInvincible()
        {
            if (_health == null) return;
            _health.Invincible = true;
            _regularMaterial = new List<Material>(_materialsToChangeWhenInvincible.Count);
            foreach (var meshRenderer in _materialsToChangeWhenInvincible) {
                _regularMaterial.Add(meshRenderer.material);
                meshRenderer.material = _invincibilityMaterial;
            }
        }

        public void OnRemoveInvincible()
        {
            if (_health == null) return;
            _health.Invincible = false;
            for (int i = 0; i < _materialsToChangeWhenInvincible.Count; ++i) {
                _materialsToChangeWhenInvincible[i].material = _regularMaterial[i];
            }
            _regularMaterial.Clear();
        }

        public void OnSpeedIncrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            _movement.OnSpeedIncrease(amount, duration, type);
        }

        public void OnSpeedDecrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            _movement.OnSpeedDecrease(amount, duration, type);
        }

        private float _rapidFireCount;

        public void Increase(float amount)
        {
            _rapidFireCount += amount;
            _fire.RapidFire(_rapidFireCount);
        }

        public void Decrease(float amount)
        {
            _rapidFireCount -= amount;
            _fire.RapidFire(_rapidFireCount);
        }
    }
}