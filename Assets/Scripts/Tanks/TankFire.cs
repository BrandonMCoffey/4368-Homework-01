using System.Collections.Generic;
using Assets.Scripts.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankFire : MonoBehaviour {
        [SerializeField] private Collider _ignoreCollider = null;
        [SerializeField] private float _ignoreDuration = 0.2f;
        [SerializeField] private Transform _turretEndPos = null;
        [SerializeField] private Bullet _bulletToFire = null;

        private List<Bullet> _bullets = new List<Bullet>();

        private void OnDisable()
        {
            _bullets.Clear();
        }

        public void Fire()
        {
            if (_bulletToFire == null) return;

            Bullet bullet = Instantiate(_bulletToFire.gameObject, _turretEndPos.position, _turretEndPos.rotation).GetComponent<Bullet>();
            if (_ignoreCollider != null) {
                bullet.TemporaryIgnore(_ignoreCollider, _ignoreDuration);
            }
            _bullets.Add(bullet);
        }
    }
}