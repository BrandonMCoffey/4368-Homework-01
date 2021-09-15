using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Mechanics.Projectiles;
using Assets.Scripts.Mechanics.Tanks.Feedback;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Tanks
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
        [Header("References")]
        [SerializeField] private TankFeedback _tankFeedback = null;

        private List<Bullet> _bullets = new List<Bullet>();
        private bool _missingTurret;

        private void Awake()
        {
            if (_turretFirePos == null) {
                _missingTurret = true;
                throw new MissingComponentException("Missing Turret Fire Transform reference on " + gameObject);
            }
        }

        private void OnDisable()
        {
            _bullets.Clear();
        }

        public void Fire()
        {
            if (_missingTurret) return;

            _bullets = _bullets.Where(item => item != null && item.isActiveAndEnabled).ToList();
            if (_hasMaximumBullets && _bullets.Count >= _maximumBullets) return;

            Bullet bullet = BulletPool.Instance.GetBullet(_bulletType);
            bullet.transform.SetPositionAndRotation(_turretFirePos.position, _turretFirePos.rotation);
            if (_ignoreCollider != null) {
                bullet.TemporaryIgnore(_ignoreCollider, _ignoreDuration);
            }
            FireFeedback();
            _bullets.Add(bullet);
        }

        private void FireFeedback()
        {
            if (_tankFeedback != null) {
                _tankFeedback.TurretFireFeedback(_turretFirePos.position, _turretFirePos.rotation);
            }
        }
    }
}