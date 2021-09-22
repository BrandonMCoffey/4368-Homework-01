namespace Interfaces
{
    public interface IDamageable
    {
        void OnTankImpact(int damageTaken);
        void OnBombDealDamage(int damageTaken);
        bool OnBulletImpact(int damageTaken);
        void OnKill();
    }
}