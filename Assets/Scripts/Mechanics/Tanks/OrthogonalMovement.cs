using UnityEngine;

namespace Mechanics.Tanks
{
    public class OrthogonalMovement : TankMovement
    {
        [SerializeField] [Range(1, 90)] private float _slowEffectWhenTurning = 45f;
        [SerializeField] private float _reverseDirectionAngle = 90f;

        private bool _moveBackwards;
        private Vector3 _moveDir;

        private Vector3 MoveDir
        {
            get
            {
                if (_moveBackwards) {
                    return -_moveDir;
                }
                return _moveDir;
            }
        }

        protected override void Move(float speed)
        {
            float angle = Vector3.Angle(MoveDir, Forward);
            float moveAmountThisFrame = MoveDir.magnitude * speed / Mathf.Max(1, angle / _slowEffectWhenTurning);
            Vector3 moveOffset = Forward * moveAmountThisFrame;
            if (_moveBackwards) {
                moveOffset = -moveOffset;
            }
            if (transform.position.y > DistFromGround) {
                moveOffset -= Vector3.up;
            }
            Rb.velocity = moveOffset;
        }

        protected override void Turn(float speed)
        {
            if (MoveDir.magnitude == 0) return;
            float angle = Vector3.Angle(MoveDir, Forward);
            if (angle > _reverseDirectionAngle) {
                _moveBackwards = !_moveBackwards;
            }

            Vector3 newDirection = Vector3.RotateTowards(Forward, MoveDir, speed * Mathf.Deg2Rad, 0);

            if (OnlyRotateArt) {
                ArtTransform.rotation = Quaternion.LookRotation(newDirection);
            } else {
                Rb.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        public override void SetMovementDirection(Vector2 dir)
        {
            dir = dir.normalized;
            _moveDir = new Vector3(dir.x, 0, dir.y);
        }
    }
}