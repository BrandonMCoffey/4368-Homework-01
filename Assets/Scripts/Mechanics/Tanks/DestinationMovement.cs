using UnityEngine;

namespace Mechanics.Tanks
{
    public class DestinationMovement : TankMovement
    {
        private Vector3 _destination;

        protected override void Move(float speed)
        {
            float dist = Vector3.Distance(transform.position, _destination);
            if (dist < 1) return;

            Vector3 moveOffset = Forward * speed;
            Rb.velocity = moveOffset;
        }

        protected override void Turn(float speed)
        {
            Quaternion currentRotation = transform.rotation;
            transform.LookAt(_destination);
            transform.rotation = Quaternion.Slerp(currentRotation, transform.rotation, speed * Time.deltaTime);
        }


        public override void SetMovementDirection(Vector2 pos)
        {
            _destination = new Vector3(pos.x, transform.position.y, pos.y);
        }
    }
}