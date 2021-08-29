using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class TankMovement : MonoBehaviour {
        [SerializeField] private float _baseMoveSpeed = 8f;
        [SerializeField] private float _turnSpeed = 10f;

        protected const float DistFromGround = 0.05f;

        protected Rigidbody Rb { get; private set; }
        public AdjustableFloat MoveSpeed { get; } = new AdjustableFloat();

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            MoveSpeed.SetBaseValue(_baseMoveSpeed);
        }

        private void FixedUpdate()
        {
            Turn(_turnSpeed);
            Move(MoveSpeed.Value);
        }

        protected abstract void Move(float speed);
        protected abstract void Turn(float speed);
        public abstract void SetMovementDirection(Vector2 dir);
    }
}