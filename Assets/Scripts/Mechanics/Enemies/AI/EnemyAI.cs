using UnityEngine;
using UnityEngine.Events;
using Utility.CustomFloats;

namespace Mechanics.Enemies.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnAimTurret = new UnityEvent<Vector2>();
        public UnityEvent OnShoot = new UnityEvent();
        public UnityEvent OnDropBomb = new UnityEvent();

        private void Update()
        {
            SetMovementDirection();
            SetAimPosition();
            SetFire();
            SetBombDrop();
        }

        protected abstract void SetMovementDirection();
        protected abstract void SetAimPosition();
        protected abstract void SetFire();
        protected abstract void SetBombDrop();

        protected void Fire()
        {
            OnShoot.Invoke();
        }

        protected void DropBomb()
        {
            OnDropBomb.Invoke();
        }


        protected Vector2 NewAimPosition()
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            Vector2 offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            return pos + offset.normalized;
        }

        protected Vector2 NewMovePosition(Vector2 moveDistance)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            bool n1 = Random.value > 0.5f;
            bool n2 = Random.value > 0.5f;
            float x = (n1 ? -1 : 1) * RandomFloat.MinMax(moveDistance);
            float y = (n2 ? -1 : 1) * RandomFloat.MinMax(moveDistance);
            return pos + new Vector2(x, y);
        }
    }
}