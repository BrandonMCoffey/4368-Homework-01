using Assets.Scripts.PlayerTank;

namespace Assets.Scripts.EnemyTanks {
    public class Killer : Enemy {
        protected override void PlayerImpact(Player player)
        {
            player.Kill();
        }
    }
}