using UnityEngine;

namespace Assets.Scripts.Mechanics.Tanks
{
    public class TankAim : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _smoothRotation = true;
        [SerializeField] private bool _lookUpAtStart = true;
        [SerializeField] private float _rotateSpeed = 5;
        [Header("References")]
        [SerializeField] private Transform _turretPivot = null;

        private Vector3 _lookAtPos;
        private bool _missingTurret;

        private void Awake()
        {
            if (_turretPivot == null) {
                _missingTurret = true;
                throw new MissingComponentException("No Turret Pivot attached to TankAim on " + gameObject);
            }
        }

        private void OnEnable()
        {
            if (_missingTurret) return;
            _lookAtPos = new Vector3(transform.position.x, _turretPivot.position.y, transform.position.z + (_lookUpAtStart ? 2 : -2));
        }

        private void Update()
        {
            if (_missingTurret) return;
            Quaternion currentRotation = _turretPivot.rotation;
            _turretPivot.LookAt(_lookAtPos);
            if (_smoothRotation) {
                _turretPivot.rotation = Quaternion.Slerp(currentRotation, _turretPivot.rotation, _rotateSpeed * Time.deltaTime);
            } else {
                _turretPivot.rotation = Quaternion.RotateTowards(currentRotation, _turretPivot.rotation, _rotateSpeed * Time.deltaTime * 10);
            }
        }

        public void SetAimPosition(Vector2 pos)
        {
            _lookAtPos = new Vector3(pos.x, _turretPivot.position.y, pos.y);
        }
    }
}