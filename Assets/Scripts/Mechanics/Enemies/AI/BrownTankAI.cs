using UnityEngine;
using Utility.CustomFloats;

namespace Mechanics.Enemies.AI
{
    public class BrownTankAI : EnemyAI
    {
        [SerializeField] private Vector2 _fireTime = new Vector2(4, 6);

        private float _fireTimer;

        private void Start()
        {
            _fireTimer = RandomFloat.MinMax(_fireTime);
            OnAimTurret.Invoke(NewAimPosition());
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
        }
    }
}