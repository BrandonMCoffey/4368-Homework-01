using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankAim : MonoBehaviour {
        [SerializeField] private Transform _turretPos = null;
        [SerializeField] private float _rotateSpeed = 5;

        private Vector3 _lookAtPos;

        private void Start()
        {
            _lookAtPos = new Vector3(transform.position.x, _turretPos.position.y, transform.position.z + 2);
        }

        private void Update()
        {
            Quaternion currentRotation = _turretPos.rotation;
            _turretPos.LookAt(_lookAtPos);
            _turretPos.rotation = Quaternion.Slerp(currentRotation, _turretPos.rotation, _rotateSpeed * Time.deltaTime);
        }

        public void SetAimPosition(Vector2 pos)
        {
            _lookAtPos = new Vector3(pos.x, _turretPos.position.y, pos.y);
        }
    }
}