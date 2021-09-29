using System.Collections.Generic;
using System.Linq;
using Mechanics.Projectiles;
using Mechanics.Tanks.Feedback;
using UnityEngine;

namespace Mechanics.Tanks
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
        [SerializeField] private float _rapidFireSpeed = 0.25f;
        [Header("References")]
        [SerializeField] private TankFeedback _tankFeedback = null;

        private List<Bullet> _bullets = new List<Bullet>();
        private bool _missingTurret;
        private float _rapidFire;
        private float _rapidFireTimer;

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

        private void Update()
        {
            if (_rapidFire > 0) {
                _rapidFireTimer += Time.deltaTime;
                if (_rapidFireTimer > _rapidFireSpeed / _rapidFire) {
                    Fire();
                    _rapidFireTimer = 0;
                }
            }
        }

        public void SetBulletType(BulletType type)
        {
            _bulletType = type;
        }

        public void RapidFire(float amount)
        {
            _rapidFire = amount;
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