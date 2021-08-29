using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player {
    public class PlayerInput : MonoBehaviour {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _tankAimMask = 0;
        [SerializeField] private float _tankAimMaxDistance = 100;

        public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnAimTurret = new UnityEvent<Vector2>();
        public UnityEvent OnShoot = new UnityEvent();

        private void Awake()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
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
            OnMoveBody?.Invoke(moveDirection);
        }

        private void SetAimPosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, _tankAimMaxDistance, _tankAimMask);
            if (hit.collider == null) return;

            OnAimTurret?.Invoke(hit.point);
        }

        private void Fire()
        {
            if (Input.GetMouseButtonDown(0)) {
                OnShoot.Invoke();
            }
        }

        public static bool IsMouseOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}