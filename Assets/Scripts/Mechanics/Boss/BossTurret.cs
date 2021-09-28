using Mechanics.Player_Systems;
using UnityEngine;
using UnityEngine.Events;
using Utility.CustomFloats;

namespace Mechanics.Boss
{
    public class BossTurret : MonoBehaviour
    {
        public UnityEvent<Vector2> OnAimTurret = new UnityEvent<Vector2>();
        public UnityEvent OnShoot = new UnityEvent();

        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Vector2 _fireTime = new Vector2(4, 6);

        private float _fireTimer;
        private bool _canShoot;

        private void Start()
        {
            _fireTimer = RandomFloat.MinMax(_fireTime);
            OnAimTurret.Invoke(AimDownwards());
            if (_playerTransform == null) {
                _playerTransform = FindObjectOfType<PlayerTank>().transform;
            }
        }

        private void Update()
        {
            SetAimPosition();
        }

        public void SetCanShoot(bool active)
        {
            _canShoot = active;
            OnAimTurret.Invoke(AimDownwards());
        }

        private void SetAimPosition()
        {
            _fireTimer -= Time.deltaTime;
            if (_fireTimer <= 0) {
                _fireTimer = RandomFloat.MinMax(_fireTime);
                if (_canShoot) {
                    OnAimTurret.Invoke(NewAimPosition());
                    OnShoot.Invoke();
                }
            }
        }

        private Vector2 NewAimPosition()
        {
            Vector3 playerPos = _playerTransform.position;
            Vector2 accuratePos = new Vector2(playerPos.x, playerPos.z);

            Vector2 offset = Random.insideUnitCircle;

            return accuratePos + offset;
        }

        private Vector2 AimDownwards()
        {
            return new Vector2(transform.position.x, -10);
        }
    }
}