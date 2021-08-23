using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts {
    public class TankController : MonoBehaviour {
        [SerializeField] private float _baseMoveSpeed = .25f;
        [SerializeField] private float _turnSpeed = 2f;

        private AdjustableFloat _moveSpeed = new AdjustableFloat();
        public AdjustableFloat AdjustMoveSpeed => _moveSpeed;
        public float MoveSpeed => _moveSpeed.Value;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _moveSpeed.SetBaseValue(_baseMoveSpeed);
        }

        private void FixedUpdate()
        {
            MoveTank();
            TurnTank();
        }

        public void MoveTank()
        {
            // calculate the move amount
            float moveAmountThisFrame = Input.GetAxis("Vertical") * MoveSpeed;
            // create a vector from amount and direction
            Vector3 moveOffset = transform.forward * moveAmountThisFrame;
            // apply vector to the rigidbody
            _rb.MovePosition(_rb.position + moveOffset);
            // technically adjusting vector is more accurate! (but more complex)
        }

        public void TurnTank()
        {
            // calculate the turn amount
            float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
            // create a Quaternion from amount and direction (x,y,z)
            Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
            // apply quaternion to the rigidbody
            _rb.MoveRotation(_rb.rotation * turnOffset);
        }
    }
}