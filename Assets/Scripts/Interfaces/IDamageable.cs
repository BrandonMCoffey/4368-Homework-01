namespace Interfaces
{
    public interface IDamageable
    {
        bool OnDamageVolume(int damage);
        void OnTankImpact(int damageTaken);
        void OnBombDealDamage(int damageTaken);
        bool OnBulletImpact(int damageTaken);
        void OnKill();
    }
}