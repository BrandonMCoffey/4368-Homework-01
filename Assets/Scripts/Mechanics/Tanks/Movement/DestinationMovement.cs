using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Mechanics.Tanks.Movement
{
    public class DestinationMovement : TankMovement
    {
        [Header("Reached Destination Settings")]
        [SerializeField] private float _distanceThreshold = 1;
        [SerializeField] private float _moveTimeout = 5;
        [Header("Force")]
        [SerializeField] private float _forceMultiplier = 8;
        [SerializeField] [Range(1f, 2f)] private float _forceDrag = 1.2f;
        [SerializeField] [Range(0.01f, 0.25f)] private float _forceCancel = 0.1f;

        public event Action ReachedDestination = delegate { };
        private bool _reachedDestination;

        private Vector3 _destination;
        private float _lastTimeMoved;

        protected override void Move(float speed)
        {
            if (_reachedDestination) return;
            float dist = Vector3.Distance(transform.position, _destination);
            if (dist < _distanceThreshold) {
                _reachedDestination = true;
                ReachedDestination.Invoke();
            }
            if (Time.time - _lastTimeMoved > _moveTimeout) {
                _reachedDestination = true;
                ReachedDestination.Invoke();
            }

            Vector3 moveOffset = Forward * speed;


            if (Force.magnitude > 0) {
                moveOffset += Force * _forceMultiplier;
                Force /= _forceDrag;
                if (Force.magnitude <= _forceCancel) {
                    Force = Vector3.zero;
                }
            }

            Rb.velocity = moveOffset;
        }

        protected override void Turn(float speed)
        {
            if (_reachedDestination) return;

            Vector2 currentPos = new Vector2(Rb.transform.position.x, Rb.transform.position.z);
            Vector2 destination = new Vector2(_destination.x, _destination.z);

            Vector2 desiredLook = destination - currentPos;
            Vector2 currentLook = new Vector2(Rb.transform.forward.x, Rb.transform.forward.z);

            float angle = Vector2.SignedAngle(desiredLook, currentLook);

            angle = Mathf.Clamp(angle, -5, 5);

            Rb.MoveRotation(Rb.rotation * Quaternion.Euler(0, angle * speed * Time.deltaTime, 0));
        }

        public override void SetMovementDirection(Vector2 pos)
        {
            _destination = new Vector3(pos.x, transform.position.y, pos.y);
            _reachedDestination = false;
            _lastTimeMoved = Time.time;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _destination);
        }
    }
}