using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class TankMovement : MonoBehaviour, IMoveable {
        [SerializeField] private float _baseMoveSpeed = 8f;
        [SerializeField] private float _turnSpeed = 10f;

        public AdjustableFloat MoveSpeed { get; } = new AdjustableFloat();

        protected const float DistFromGround = 0.05f;

        protected Rigidbody Rb { get; private set; }

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

        public void OnSpeedIncrease(float amount, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            MoveSpeed.IncreaseValue(type, amount);
        }

        public void OnSpeedIncrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            StartCoroutine(MoveSpeed.TemporaryIncrease(type, amount, duration));
        }

        public void OnSpeedDecrease(float amount, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            MoveSpeed.DecreaseValue(type, amount);
        }

        public void OnSpeedDecrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            StartCoroutine(MoveSpeed.TemporaryDecrease(type, amount, duration));
        }
    }
}