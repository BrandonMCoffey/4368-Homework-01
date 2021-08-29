using Assets.Scripts.Player;

namespace Assets.Scripts.Enemies {
    public class Killer : Enemy {
        protected override void PlayerImpact(PlayerTank playerTank)
        {
            playerTank.Health.Kill();
        }
    }
}