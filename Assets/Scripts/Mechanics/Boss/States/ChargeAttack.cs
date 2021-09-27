using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class ChargeAttack : IState
    {
        private BossStateMachine _stateMachine;
        private BossMovement _bossMovement;

        public ChargeAttack(BossStateMachine stateMachine, BossMovement bossMovement)
        {
            _stateMachine = stateMachine;
            _bossMovement = bossMovement;
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void FixedTick()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}