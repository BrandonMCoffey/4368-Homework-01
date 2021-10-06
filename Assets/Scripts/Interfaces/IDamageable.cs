using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        bool OnDamageVolume(int damage);
        void OnTankImpact(int damageTaken);
        void OnBombDealDamage(int damageTaken);
        bool OnBulletImpact(int damageTaken, Vector3 forward);
        void OnKill();
    }
}