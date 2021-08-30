namespace Assets.Scripts.Interfaces {
    public interface IDamageable {
        void OnTankImpact(int damageTaken);
        void OnBulletImpact(int damageTaken);
        void OnKill();
    }
}