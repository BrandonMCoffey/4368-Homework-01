using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class PlatformSummoning : IState
    {
        private BossStateMachine _stateMachine;

        public PlatformSummoning(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
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