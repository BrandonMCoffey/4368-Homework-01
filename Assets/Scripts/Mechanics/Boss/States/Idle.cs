using UnityEngine;
using Utility.CustomFloats;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class Idle : IState
    {
        private BossStateMachine _stateMachine;
        private Vector2 _idleTimeMinMax;

        private float _idleTime;
        private float _timer;
        private bool _debug;
        private float _idleTimeMultiplier = 1;

        public Idle(BossStateMachine stateMachine, BossAiData data)
        {
            _stateMachine = stateMachine;
            _idleTimeMinMax = data.IdleTimeMinMax;
            _debug = data.Debug;
        }

        public void SetEscalation()
        {
            _idleTimeMultiplier = 0.5f;
        }

        public void Enter()
        {
            _idleTime = RandomFloat.MinMax(_idleTimeMinMax) * _idleTimeMultiplier;
            if (_debug) Debug.Log("Idle: Idle for " + _idleTime);
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