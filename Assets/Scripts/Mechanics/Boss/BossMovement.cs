using Mechanics.Tanks.Feedback;
using UnityEngine;

namespace Mechanics.Boss
{
    public class BossMovement : MonoBehaviour
    {
        [SerializeField] private Transform _mainTransform;
        [SerializeField] private BossFeedback _feedback = null;
        [SerializeField] private float _movementSpeed = 2;
        [SerializeField] private float _escalationSpeedMultiplier = 2;
        [SerializeField] private LayerMask _wallMask = 1;
        [SerializeField] private Vector3 _chargeDirection = Vector3.right;
        [SerializeField] private float _chargeWallOffset = 0.25f;

        public Transform MainTransform => _mainTransform;
        private Vector3 _originalRotation;

        private Vector3 _startCharge;
        private Vector3 _endCharge;
        private float _chargeDelta;
        private float _speedBonus = 1;

        private void Awake()
        {
            NullTest();
            _originalRotation = MainTransform.eulerAngles;
        }

        public void SetEscalation()
        {
            _speedBonus = _escalationSpeedMultiplier;
        }

        public bool MoveTowards(Vector3 destination)
        {
            Vector3 newPosition = Vector3.MoveTowards(_mainTransform.position, destination, _movementSpeed * _speedBonus * Time.deltaTime);
            _mainTransform.position = newPosition;

            return Vector3.Distance(_mainTransform.position, destination) < 0.01f;
        }

        public void Rotate(float rotation)
        {
            Vector3 rot = _originalRotation;
            rot.y += rotation;
            _mainTransform.rotation = Quaternion.Euler(rot);
        }

        public void StartCharge()
        {
            _chargeDelta = 0;
            _startCharge = transform.position;
            Ray chargeRay = new Ray(_startCharge, _chargeDirection);
            Physics.Raycast(chargeRay, out var hit, 100, _wallMask);
            if (hit.collider != null) {
                _endCharge = hit.point - _chargeDirection * _chargeWallOffset;
            } else {
                Debug.Log("Warning: No Wall Detected, assuming 10m distance for boss charge", gameObject);
                _endCharge = _startCharge + 10 * _chargeDirection;
            }
        }

        public void StartSecondCharge()
        {
            _chargeDelta = 1 - _chargeDelta;
        }

        public bool Charge(float speed)
        {
            _chargeDelta += speed * _speedBonus;
            Vector3 current = Vector3.Lerp(_startCharge, _endCharge, _chargeDelta);
            _mainTransform.position = current;
            if (_chargeDelta > 1) {
                _chargeDelta = 0;
                return true;
            }
            return false;
        }

        public void Impact()
        {
            _feedback.ShakeScreen();
        }

        public bool Retreat(float speed)
        {
            _chargeDelta += speed * _speedBonus;
            Vector3 current = Vector3.Lerp(_endCharge, _startCharge, _chargeDelta);
            _mainTransform.position = current;
            if (_chargeDelta > 1) {
                return true;
            }
            return false;
        }

        private void NullTest()
        {
            if (_mainTransform == null) {
                _mainTransform = transform.parent;
                if (_mainTransform == null) {
                    _mainTransform = transform;
                }
            }
        }
    }
}