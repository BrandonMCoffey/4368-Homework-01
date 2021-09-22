using Mechanics.Tanks.Movement;
using UnityEngine;
using Utility.CustomFloats;

namespace Mechanics.Enemies.AI
{
    public class YellowTankAI : EnemyAI
    {
        [SerializeField] private Vector2 _fireTime = new Vector2(10, 20);
        [SerializeField] private Vector2 _bombTime = new Vector2(3, 8);
        [SerializeField] private Vector2 _moveDistance = new Vector2(3, 4);

        [SerializeField] private DestinationMovement _destinationMovement = null;

        private float _fireTimer;
        private float _bombTimer;

        private void OnEnable()
        {
            _fireTimer = RandomFloat.MinMax(_fireTime);
            _bombTimer = RandomFloat.MinMax(_bombTime) / 2;

            OnAimTurret.Invoke(NewAimPosition());
            OnMoveBody.Invoke(NewMovePosition(_moveDistance));

            if (_destinationMovement != null) {
                _destinationMovement.ReachedDestination += SetNewDestination;
            }
        }

        private void OnDisable()
        {
            if (_destinationMovement != null) {
                _destinationMovement.ReachedDestination -= SetNewDestination;
            }
        }

        private void SetNewDestination()
        {
            OnMoveBody.Invoke(NewMovePosition(_moveDistance));
        }

        protected override void SetMovementDirection()
        {
        }

        protected override void SetAimPosition()
        {
            _fireTimer -= Time.deltaTime;
            if (_fireTimer <= 0) {
                _fireTimer = RandomFloat.MinMax(_fireTime);
                OnAimTurret.Invoke(NewAimPosition());
                Fire();
            }
        }

        protected override void SetFire()
        {
        }

        protected override void SetBombDrop()
        {
            _bombTimer -= Time.deltaTime;
            if (_bombTimer <= 0) {
                _bombTimer = RandomFloat.MinMax(_bombTime);
                DropBomb();
            }
        }
    }
}