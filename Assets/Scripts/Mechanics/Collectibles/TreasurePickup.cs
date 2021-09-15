using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Collectibles
{
    public class TreasurePickup : CollectibleBase, ILockable
    {
        [SerializeField] private Vector3 _centerOffset = new Vector3(0, 0.5f, 0);
        [SerializeField] private float _upDownSpeed = 0.1f;
        [SerializeField] private float _upDownAmplitude = 0.4f;
        [Header("Treasure Settings")]
        [SerializeField] private int _value = 1;

        private bool _locked;
        private float _spin;
        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position + _centerOffset;
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

        protected override void Movement(Transform obj)
        {
            base.Movement(obj);
            if (_locked) return;
            _spin += _upDownSpeed;
            obj.position = _startPos + Vector3.up * Mathf.Sin(_spin) * _upDownAmplitude;
        }

        public void Lock(bool active = true)
        {
            if (!active) {
                _startPos = transform.position + _centerOffset;
            }
            _locked = active;
            Collider.enabled = !active;
        }
    }
}