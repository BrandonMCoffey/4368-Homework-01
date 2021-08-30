using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class TreasurePickup : CollectibleBase {
        [SerializeField] private float _upDownSpeed = 0.1f;
        [SerializeField] private float _upDownAmplitude = 0.4f;
        [Header("Treasure Settings")]
        [SerializeField] private int _value = 1;

        private float _spin;
        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position;
        }

        protected override bool OnCollect(GameObject other)
        {
            IInventory<Treasure> inventory = other.GetComponent<IInventory<Treasure>>();
            if (inventory == null) {
                return false;
            }
            // Permanently Increase the speed of the other object
            inventory.OnCollect(new Treasure(_value));

            return true;
        }

        protected override void Movement(Rigidbody rb)
        {
            base.Movement(rb);
            _spin += _upDownSpeed;
            rb.MovePosition(_startPos + Vector3.up * Mathf.Sin(_spin) * _upDownAmplitude);
        }
    }
}