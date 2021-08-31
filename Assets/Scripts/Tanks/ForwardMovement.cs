using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class ForwardMovement : TankMovement {
        private float _forwardAmount;
        private float _turnAmount;

        protected override void Move(float speed)
        {
            // calculate the move amount
            float moveAmountThisFrame = _forwardAmount * speed;
            // create a vector from amount and direction
            Vector3 moveOffset = Forward * moveAmountThisFrame;
            if (transform.position.y > DistFromGround) {
                moveOffset -= Vector3.up;
            }
            // apply vector to the rigidbody
            Rb.velocity = moveOffset;
            //Rb.MovePosition(Rb.position + moveOffset);
            // technically adjusting vector is more accurate! (but more complex)
        }

        protected override void Turn(float speed)
        {
            // calculate the turn amount
            float turnAmountThisFrame = _turnAmount * speed;

            // Add the turn amount to angular velocity
            //Rb.angularVelocity = new Vector3(0, turnAmountThisFrame, 0);

            // create a Quaternion from amount and direction (x,y,z)
            Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
            // apply quaternion to the rigidbody
            Rb.MoveRotation(Rb.rotation * turnOffset);

            if (OnlyRotateArt) {
                ArtTransform.rotation = Rb.rotation * turnOffset;
            } else {
                Rb.MoveRotation(Rb.rotation * turnOffset);
            }
        }


        public override void SetMovementDirection(Vector2 dir)
        {
            _forwardAmount = dir.y;
            _turnAmount = dir.x;
        }
    }
}