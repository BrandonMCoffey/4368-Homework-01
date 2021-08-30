using Assets.Scripts.Bullets;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankFire : MonoBehaviour {
        [SerializeField] private Transform _turretEndPos = null;
        [SerializeField] private Bullet _bulletToFire = null;

        public void Fire()
        {
            if (_bulletToFire == null) return;
            Instantiate(_bulletToFire.gameObject, _turretEndPos.position, _turretEndPos.rotation);
        }
    }
}