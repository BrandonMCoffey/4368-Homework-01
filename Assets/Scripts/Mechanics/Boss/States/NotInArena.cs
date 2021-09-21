using UnityEngine;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class NotInArena : IState
    {
        private BossStateMachine _bossStateMachine;
        private BossPlatformController _platformController;

        public NotInArena(BossStateMachine bossStateMachine, BossPlatformController platformController)
        {
            _bossStateMachine = bossStateMachine;
            _platformController = platformController;
        }

        private Vector2 _idleTimeMinMax = new Vector2(2f, 6f);
        private float _idleTime;
        private float _timer;

        public void Enter()
        {
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