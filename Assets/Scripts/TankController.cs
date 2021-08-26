using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts {
    public class TankController : EntityMovement {
        protected override void Move(float speed)
        {
            // calculate the move amount
            float moveAmountThisFrame = Input.GetAxis("Vertical") * speed;
            // create a vector from amount and direction
            Vector3 moveOffset = transform.forward * moveAmountThisFrame;
            // apply vector to the rigidbody
            Rb.velocity = moveOffset;
            //Rb.MovePosition(Rb.position + moveOffset);
            // technically adjusting vector is more accurate! (but more complex)
        }

        protected override void Turn(float speed)
        {
            // calculate the turn amount
            float turnAmountThisFrame = Input.GetAxisRaw("Horizontal") * speed;
            // create a Quaternion from amount and direction (x,y,z)
            //Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
            // apply quaternion to the rigidbody
            Rb.angularVelocity = new Vector3(0, turnAmountThisFrame, 0);
            //Rb.MoveRotation(Rb.rotation * turnOffset);
        }
    }
}