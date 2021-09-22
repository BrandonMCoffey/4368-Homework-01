using UnityEngine;
using Utility.CustomFloats;

namespace Mechanics.Enemies.AI
{
    public class GrayTankAI : EnemyAI
    {
        [SerializeField] private Vector2 _fireTime = new Vector2(4, 6);
        [SerializeField] private Vector2 _moveTime = new Vector2(4, 6);
        [SerializeField] private Vector2 _moveDistance = new Vector2(3, 4);

        private float _fireTimer;
        private float _moveTimer;

        private void OnEnable()
        {
            _fireTimer = RandomFloat.MinMax(_fireTime);
            OnAimTurret.Invoke(NewAimPosition());
        }

        protected override void SetMovementDirection()
        {
            _moveTimer -= Time.deltaTime;
            if (_moveTimer <= 0) {
                _moveTimer = RandomFloat.MinMax(_moveTime);
                OnMoveBody.Invoke(NewMovePosition(_moveDistance));
            }
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
        }
    }
}