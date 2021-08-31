using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankAim : MonoBehaviour {
        [SerializeField] private Transform _turretPos = null;
        [SerializeField] private bool _smoothRotation = true;
        [SerializeField] private float _rotateSpeed = 5;

        private Vector3 _lookAtPos;

        private void Awake()
        {
            _lookAtPos = new Vector3(transform.position.x, _turretPos.position.y, transform.position.z + 2);
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

        public void SetAimPosition(Vector2 pos)
        {
            _lookAtPos = new Vector3(pos.x, _turretPos.position.y, pos.y);
        }
    }
}