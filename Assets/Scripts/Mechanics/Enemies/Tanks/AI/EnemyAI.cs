using UnityEngine;
using UnityEngine.Events;

namespace Mechanics.Enemies.Tanks.AI
{
    public abstract class EnemyAI : MonoBehaviour
    {
        public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnAimTurret = new UnityEvent<Vector2>();
        public UnityEvent OnShoot = new UnityEvent();

        private void Update()
        {
            SetMovementDirection();
            SetAimPosition();
            SetFire();
        }

        protected abstract void SetMovementDirection();
        protected abstract void SetAimPosition();
        protected abstract void SetFire();

        protected void Fire()
        {
            OnShoot.Invoke();
        }
    }
}