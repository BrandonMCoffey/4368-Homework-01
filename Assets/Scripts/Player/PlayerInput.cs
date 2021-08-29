using Assets.Scripts.Tanks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player {
    [RequireComponent(typeof(TankMovement))]
    [RequireComponent(typeof(TankAim))]
    [RequireComponent(typeof(TankFire))]
    public class PlayerInput : MonoBehaviour {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _tankAimMask = 0;
        [SerializeField] private float _tankAimMaxDistance = 100;

        private TankMovement _movementBase;
        private TankAim _aim;
        private TankFire _fire;

        private void Awake()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;

            _movementBase = GetComponent<TankMovement>();
            _aim = GetComponent<TankAim>();
            _fire = GetComponent<TankFire>();
        }

        private void Update()
        {
            SetMovementDirection();

            if (IsMouseOverUI()) return;

            SetAimPosition();
            Fire();
        }

        private void SetMovementDirection()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 moveDirection = new Vector2(moveHorizontal, moveVertical);
            moveDirection = moveDirection.normalized;

            _movementBase.SetMovementDirection(moveDirection);
        }

        private void SetAimPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, _tankAimMaxDistance, _tankAimMask);
            if (hit.collider == null) return;

            _aim.SetAimPosition(hit.point);
        }

        private void Fire()
        {
            if (Input.GetMouseButtonDown(0)) {
                _fire.Fire();
            }
        }

        public static bool IsMouseOverUI()
        {
            if (EventSystem.current == null) return false;
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}