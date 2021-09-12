using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Tanks
{
    public class TankFire : MonoBehaviour
    {
        [Header("Necessary Bullet Details")]
        [SerializeField] private Transform _turretFirePos = null;
        [SerializeField] private BulletType _bulletType = BulletType.Normal;
        [Header("Ignore Collision When Firing")]
        [SerializeField] private Collider _ignoreCollider = null;
        [SerializeField] private float _ignoreDuration = 0.2f;
        [Header("Limits")]
        [SerializeField] private bool _hasMaximumBullets = false;
        [SerializeField] private float _maximumBullets = 5;

        private List<Bullet> _bullets = new List<Bullet>();

        private void Awake()
        {
            if (_turretFirePos == null) {
                DebugHelper.Error(gameObject, "Missing Bullet Information To Fire");
            }
        }

        private void OnDisable()
        {
            _bullets.Clear();
        }

        public void Fire()
        {
            if (_turretFirePos == null) return;

            _bullets = _bullets.Where(item => item != null && item.isActiveAndEnabled).ToList();
            if (_hasMaximumBullets && _bullets.Count >= _maximumBullets) return;

            Bullet bullet = BulletPool.Instance.GetBullet(_bulletType);
            bullet.transform.SetPositionAndRotation(_turretFirePos.position, _turretFirePos.rotation);
            if (_ignoreCollider != null) {
                bullet.TemporaryIgnore(_ignoreCollider, _ignoreDuration);
            }
            bullet.FireFeedback(_turretFirePos.position, _turretFirePos.rotation);
            _bullets.Add(bullet);
        }
    }
}