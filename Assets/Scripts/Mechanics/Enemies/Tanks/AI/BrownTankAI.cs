using UnityEngine;

namespace Assets.Scripts.Mechanics.Enemies.Tanks.AI
{
    public class BrownTankAI : EnemyAI
    {
        [SerializeField] private Vector2 _fireTime = new Vector2(4, 6);

        private float _fireTimer;

        private void Start()
        {
            _fireTimer = RandomMinMax(_fireTime);
            OnAimTurret.Invoke(NewAimPosition());
        }

        protected override void SetMovementDirection()
        {
        }

        protected override void SetAimPosition()
        {
            _fireTimer -= Time.deltaTime;
            if (_fireTimer <= 0) {
                _fireTimer = RandomMinMax(_fireTime);
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

        private static float RandomMinMax(Vector2 minMax)
        {
            return Random.Range(minMax.x, Mathf.Max(minMax.x, minMax.y));
        }
    }
}