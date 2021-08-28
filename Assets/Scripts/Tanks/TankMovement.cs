using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class TankMovement : MonoBehaviour {
        [SerializeField] private float _baseMoveSpeed = .25f;
        [SerializeField] private float _turnSpeed = 2f;

        private AdjustableFloat _moveSpeed = new AdjustableFloat();
        public AdjustableFloat AdjustMoveSpeed => _moveSpeed;

        protected Rigidbody Rb { get; private set; }

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _moveSpeed.SetBaseValue(_baseMoveSpeed);
        }

        private void FixedUpdate()
        {
            Move(_moveSpeed.Value);
            Turn(_turnSpeed);
        }

        protected abstract void Move(float speed);
        protected abstract void Turn(float speed);
    }
}