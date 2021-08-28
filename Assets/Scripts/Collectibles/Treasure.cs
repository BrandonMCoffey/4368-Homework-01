using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Collectibles {
    public class Treasure : CollectibleBase {
        [SerializeField] private int _value = 1;
        [SerializeField] private float _upDownSpeed = 0.1f;
        [SerializeField] private float _upDownAmplitude = 0.4f;

        private float _spin;
        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.position;
        }

        protected override bool Collect(PlayerTank playerTank)
        {
            playerTank.GetInventory().AddTreasure(_value);
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