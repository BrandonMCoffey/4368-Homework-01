using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Mechanics.Player_Systems
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fireSpeed = 0.1f;

        [Header("Key Inputs")]
        [SerializeField] private KeyCode _fireKey = KeyCode.Space;
        [SerializeField] private bool _altFireLeftClick = true;

        [Header("Axis Inputs")]
        [SerializeField] private bool _axisRaw = true;
        [SerializeField] private string _moveLeftRight = "Horizontal";
        [SerializeField] private string _moveUpDown = "Vertical";

        [Header("References")]
        [SerializeField] private LayerMask _tankAimMask = 0;
        [SerializeField] private float _tankAimMaxDistance = 100;

        public UnityEvent<Vector2> OnMoveBody = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnAimTurret = new UnityEvent<Vector2>();
        public UnityEvent OnShoot = new UnityEvent();

        private Camera _mainCamera;
        private float _fireTimer;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            SetMovementDirection();
            SetAimPosition();
            Fire();
        }

        private void SetMovementDirection()
        {
            float moveHorizontal = _axisRaw ? Input.GetAxisRaw(_moveLeftRight) : Input.GetAxis(_moveLeftRight);
            float moveVertical = _axisRaw ? Input.GetAxisRaw(_moveUpDown) : Input.GetAxis(_moveUpDown);

            Vector2 moveDirection = new Vector2(moveHorizontal, moveVertical);
            OnMoveBody?.Invoke(moveDirection);
        }

        private void SetAimPosition()
        {
            if (IsMouseOverUI()) return;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, _tankAimMaxDistance, _tankAimMask);
            if (hit.collider == null) return;

            Vector2 pos = new Vector2(hit.point.x, hit.point.z);
            OnAimTurret?.Invoke(pos);
        }

        private void Fire()
        {
            if (Input.GetKeyDown(_fireKey)) {
                OnShoot.Invoke();
                _fireTimer = 0;
            } else if (_altFireLeftClick && !IsMouseOverUI() && Input.GetMouseButtonDown(0)) {
                OnShoot.Invoke();
                _fireTimer = 0;
            } else if (Input.GetKey(_fireKey) || (_altFireLeftClick && !IsMouseOverUI() && Input.GetMouseButton(0))) {
                _fireTimer += Time.deltaTime;
                if (_fireTimer > _fireSpeed) {
                    OnShoot.Invoke();
                    _fireTimer = 0;
                }
            }
        }

        public static bool IsMouseOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}