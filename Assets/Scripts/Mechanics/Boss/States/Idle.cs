using Assets.Scripts.Utility.CustomFloats;
using Assets.Scripts.Utility.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Boss.States
{
    public class Idle : IState
    {
        private BossStateMachine _stateMachine;

        public Idle(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private Vector2 _idleTimeMinMax = new Vector2(2f, 6f);
        private float _idleTime;
        private float _timer;

        public void Enter()
        {
            _idleTime = RandomFloat.MinMax(_idleTimeMinMax);
            _timer = 0;
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer >= _idleTime) {
                _stateMachine.BossFinishedIdle();
            }
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
        }
    }
}