using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class TankMovement : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _baseMoveSpeed = 8f;
        [SerializeField] private float _turnSpeed = 10f;
        [SerializeField] private bool _onlyRotateArt = true;
        [SerializeField] private Transform _artTransform = null;

        public AdjustableFloat MoveSpeed { get; } = new AdjustableFloat();

        protected const float DistFromGround = 0.05f;

        protected Rigidbody Rb { get; private set; }

        protected bool OnlyRotateArt => _onlyRotateArt && _artTransform != null;
        protected Transform ArtTransform => _artTransform;

        protected Vector3 Forward
        {
            get
            {
                if (_onlyRotateArt && _artTransform != null) {
                    return _artTransform.forward;
                }
                return transform.forward;
            }
        }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            if (OnlyRotateArt) {
                Rb.freezeRotation = true;
            }
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

        public void OnSpeedIncrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            if (duration < 0) MoveSpeed.IncreaseValue(type, amount);
            else StartCoroutine(MoveSpeed.TemporaryIncrease(type, amount, duration));
        }

        public void OnSpeedDecrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            if (duration < 0) MoveSpeed.DecreaseValue(type, amount);
            else StartCoroutine(MoveSpeed.TemporaryDecrease(type, amount, duration));
        }
    }
}