using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Enemies.Tanks.AI
{
    public class GrayTankAI : EnemyAI
    {
        [SerializeField] private Vector2 _fireTime = new Vector2(4, 6);
        [SerializeField] private Vector2 _moveTime = new Vector2(4, 6);
        [SerializeField] private Vector2 _moveDistance = new Vector2(3, 4);

        private float _fireTimer;
        private float _moveTimer;

        private void Start()
        {
            _fireTimer = RandomFloat.MinMax(_fireTime);
            OnAimTurret.Invoke(NewAimPosition());
        }

        protected override void SetMovementDirection()
        {
            _moveTimer -= Time.deltaTime;
            if (_moveTimer <= 0) {
                _moveTimer = RandomFloat.MinMax(_fireTime);
                OnMoveBody.Invoke(NewMovePosition());
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

        private Vector2 NewAimPosition()
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            Vector2 offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            return pos + offset.normalized;
        }

        private Vector2 NewMovePosition()
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            bool n1 = Random.value > 0.5f;
            bool n2 = Random.value > 0.5f;
            float x = (n1 ? -1 : 1) * RandomFloat.MinMax(_moveDistance);
            float y = (n2 ? -1 : 1) * RandomFloat.MinMax(_moveDistance);
            return pos + new Vector2(x, y);
        }
    }
}