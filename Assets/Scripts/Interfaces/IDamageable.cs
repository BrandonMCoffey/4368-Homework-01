namespace Interfaces
{
    public interface IDamageable
    {
        void OnTankImpact(int damageTaken);
        bool OnBulletImpact(int damageTaken);
        void OnKill();
    }
}