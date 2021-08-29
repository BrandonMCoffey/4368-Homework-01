using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankMovement : MonoBehaviour {
        [SerializeField] private float _baseMoveSpeed = 8f;
        [SerializeField] private float _turnSpeed = 10f;
        [SerializeField] [Range(1, 90)] private float _slowEffectWhenTurning = 45f;
        [SerializeField] private float _reverseDirectionAngle = 90f;

        private const float DistFromGround = 0.05f;

        private Rigidbody _rb;
        private Vector3 _moveDir;

        public AdjustableFloat MoveSpeed { get; } = new AdjustableFloat();

        private Vector3 MoveDir {
            get {
                if (_moveBackwards) {
                    return -_moveDir;
                }
                return _moveDir;
            }
        }

        private bool _moveBackwards;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            MoveSpeed.SetBaseValue(_baseMoveSpeed);
        }

        private void FixedUpdate()
        {
            Turn();
            Move();
        }

        private void Move()
        {
            float angle = Vector3.Angle(MoveDir, transform.forward);
            float moveAmountThisFrame = MoveDir.magnitude * MoveSpeed.Value / Mathf.Max(1, angle / _slowEffectWhenTurning);
            Vector3 moveOffset = transform.forward * moveAmountThisFrame;
            if (_moveBackwards) {
                moveOffset = -moveOffset;
            }
            if (transform.position.y > DistFromGround) {
                moveOffset -= Vector3.up;
            }
            _rb.velocity = moveOffset;
        }

        private void Turn()
        {
            if (MoveDir.magnitude == 0) return;
            float angle = Vector3.Angle(MoveDir, transform.forward);
            if (angle > _reverseDirectionAngle) {
                _moveBackwards = !_moveBackwards;
            }

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, MoveDir, _turnSpeed * Mathf.Deg2Rad, 0);
            _rb.rotation = Quaternion.LookRotation(newDirection);
        }

        public void SetMovementDirection(Vector2 dir)
        {
            _moveDir = new Vector3(dir.x, 0, dir.y);
        }
    }
}