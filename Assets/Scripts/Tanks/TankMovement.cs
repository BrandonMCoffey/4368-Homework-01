using System.Collections;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class TankMovement : MonoBehaviour, IMoveable {
        [SerializeField] private float _baseMoveSpeed = 8f;
        [SerializeField] private float _turnSpeed = 10f;
        [SerializeField] private ParticleSystem _slowedDownEffects = null;
        [SerializeField] private ParticleSystem _speedBoostEffects = null;
        [SerializeField] private float _particleMultiplier = 10;

        private AdjustableFloat _moveSpeed = new AdjustableFloat();

        protected const float DistFromGround = 0.05f;

        protected Rigidbody Rb { get; private set; }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _moveSpeed.SetBaseValue(_baseMoveSpeed);
        }

        private void Update()
        {
            // TODO: Offload to other script and use Events (DON'T UPDATE EVERY FRAME)
            SpeedBoostFeedback();
            SlowedDownFeedback();
        }

        private void SpeedBoostFeedback()
        {
            if (_speedBoostEffects == null) return;
            ParticleSystem.EmissionModule emission = _speedBoostEffects.emission;
            emission.rateOverTime = _moveSpeed.ActivePositiveEffects * _particleMultiplier;
        }

        private void SlowedDownFeedback()
        {
            if (_slowedDownEffects == null) return;
            ParticleSystem.EmissionModule emission = _slowedDownEffects.emission;
            emission.rateOverTime = _moveSpeed.ActiveNegativeEffects * _particleMultiplier;
        }

        private void FixedUpdate()
        {
            Turn(_turnSpeed);
            Move(_moveSpeed.Value);
        }

        protected abstract void Move(float speed);
        protected abstract void Turn(float speed);
        public abstract void SetMovementDirection(Vector2 dir);

        public void OnSpeedIncrease(float amount, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            _moveSpeed.IncreaseValue(type, amount);
        }

        public void OnSpeedIncrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            StartCoroutine(_moveSpeed.TemporaryIncrease(type, amount, duration));
        }

        public void OnSpeedDecrease(float amount, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            _moveSpeed.DecreaseValue(type, amount);
        }

        public void OnSpeedDecrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw)
        {
            StartCoroutine(_moveSpeed.TemporaryDecrease(type, amount, duration));
        }
    }
}