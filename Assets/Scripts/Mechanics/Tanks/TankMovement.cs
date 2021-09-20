using Interfaces;
using Mechanics.Tanks.Feedback;
using UnityEngine;
using Utility.CustomFloats;

namespace Mechanics.Tanks
{
    public abstract class TankMovement : MonoBehaviour, IMoveable
    {
        [Header("References")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _artTransform = null;
        [SerializeField] private bool _onlyRotateArt = true;
        [SerializeField] private TankFeedback _tankFeedback;
        [Header("Settings")]
        [SerializeField] private float _baseMoveSpeed = 5;
        [SerializeField] private float _turnSpeed = 8;

        public AdjustableFloat MoveSpeed { get; } = new AdjustableFloat();

        protected const float DistFromGround = 0.05f;

        protected Rigidbody Rb => _rigidbody;

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
            if (Rb == null) {
                _rigidbody = GetComponent<Rigidbody>();
                if (Rb == null) {
                    throw new MissingComponentException("No Rigidbody Attached to  Tank Movement on " + gameObject);
                }
            }
            if (OnlyRotateArt) {
                Rb.freezeRotation = true;
            }
        }

        private void OnEnable()
        {
            MoveSpeed.SetBaseValue(_baseMoveSpeed);
            if (_tankFeedback != null) MoveSpeed.ActiveEffects += _tankFeedback.SetMovementEffects;
        }

        private void OnDisable()
        {
            if (_tankFeedback != null) MoveSpeed.ActiveEffects -= _tankFeedback.SetMovementEffects;
        }

        private void FixedUpdate()
        {
            if (Rb == null) return;
            Turn(_turnSpeed);
            Move(MoveSpeed.Value);
            MovementFeedback();
        }

        protected abstract void Move(float speed);
        protected abstract void Turn(float speed);
        public abstract void SetMovementDirection(Vector2 dir);

        public void OnSpeedIncrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            if (duration < 0) {
                MoveSpeed.IncreaseValue(type, amount);
            } else {
                StartCoroutine(MoveSpeed.TemporaryIncrease(type, amount, duration));
            }
        }

        public void OnSpeedDecrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            if (duration < 0) {
                MoveSpeed.DecreaseValue(type, amount);
            } else {
                StartCoroutine(MoveSpeed.TemporaryDecrease(type, amount, duration));
            }
        }

        private void MovementFeedback()
        {
            if (_tankFeedback != null) _tankFeedback.SetMovementSpeed(Rb.velocity.magnitude / MoveSpeed.Value);
        }
    }
}