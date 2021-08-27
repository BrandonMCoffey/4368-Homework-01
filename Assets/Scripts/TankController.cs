using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts {
    public class TankController : EntityMovement {
        [SerializeField] private ParticleSystem _slowedDownEffects = null;
        [SerializeField] private float _slowedDownEffectsMultiplier = 10;

        private const float DistFromGround = 0.05f;

        private void Update()
        {
            ParticleSystem.MainModule main = _slowedDownEffects.main;
            ParticleSystem.EmissionModule emission = _slowedDownEffects.emission;
            main.startSpeedMultiplier = Rb.velocity.magnitude;
            emission.rateOverTime = AdjustMoveSpeed.ActiveEffects * _slowedDownEffectsMultiplier;
        }

        protected override void Move(float speed)
        {
            // calculate the move amount
            float moveAmountThisFrame = Input.GetAxis("Vertical") * speed;
            // create a vector from amount and direction
            Vector3 moveOffset = transform.forward * moveAmountThisFrame;
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
            float turnAmountThisFrame = Input.GetAxisRaw("Horizontal") * speed;

            // Add the turn amount to angular velocity
            //Rb.angularVelocity = new Vector3(0, turnAmountThisFrame, 0);

            // create a Quaternion from amount and direction (x,y,z)
            Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
            // apply quaternion to the rigidbody
            Rb.MoveRotation(Rb.rotation * turnOffset);
        }
    }
}