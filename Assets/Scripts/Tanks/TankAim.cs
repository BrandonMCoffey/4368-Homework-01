using UnityEngine;

namespace Assets.Scripts.Tanks
{
    public class TankAim : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _smoothRotation = true;
        [SerializeField] private bool _lookUpAtStart = true;
        [SerializeField] private float _rotateSpeed = 5;
        [Header("References")]
        [SerializeField] private Transform _turretPos = null;
        [Header("Debug")]
        [SerializeField] private bool _debugLine = false;
        [SerializeField] private Color _debugColor = Color.red;

        private Vector3 _lookAtPos;

        private void OnEnable()
        {
            _lookAtPos = new Vector3(transform.position.x, _turretPos.position.y, transform.position.z + (_lookUpAtStart ? 2 : -2));
        }

        private void Update()
        {
            Quaternion currentRotation = _turretPos.rotation;
            _turretPos.LookAt(_lookAtPos);
            if (_smoothRotation) {
                _turretPos.rotation = Quaternion.Slerp(currentRotation, _turretPos.rotation, _rotateSpeed * Time.deltaTime);
            } else {
                _turretPos.rotation = Quaternion.RotateTowards(currentRotation, _turretPos.rotation, _rotateSpeed * Time.deltaTime * 10);
            }
        }

        private void OnDrawGizmos()
        {
            if (_debugLine) {
                Gizmos.color = _debugColor;
                Gizmos.DrawLine(_turretPos.position, _lookAtPos);
            }
        }

        public void SetAimPosition(Vector2 pos)
        {
            _lookAtPos = new Vector3(pos.x, _turretPos.position.y, pos.y);
        }
    }
}