using UnityEngine;
using Utility.CustomFloats;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class NotInArena : IState
    {
        private BossStateMachine _stateMachine;
        private BossPlatformController _platformController;

        private Vector2 _idleTimeMinMax;
        private float _idleTime;
        private float _timer;
        private bool _finishedIdle;
        private bool _debug;

        public NotInArena(BossStateMachine bossStateMachine, BossPlatformController platformController, BossAiData data)
        {
            _stateMachine = bossStateMachine;
            _platformController = platformController;
            _idleTimeMinMax = data.OutsideArenaMinMax;
            _debug = data.Debug;
        }

        public void Enter()
        {
            if (!_platformController.IsOnPlatform) {
                if (_debug) Debug.Log("Idle: Not on Platform, Reverting...");
                _stateMachine.RevertToPreviousState();
                return;
            }
            float timeToLower = _platformController.Lower(_stateMachine);

            _idleTime = RandomFloat.MinMax(_idleTimeMinMax);
            if (_debug) Debug.Log("NotInArenaIdle: Idle for " + _idleTime);
            _timer = -timeToLower;
            _finishedIdle = false;
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer < _idleTime) return;

            if (!_finishedIdle) {
                float timeToRaise = _platformController.Raise(_stateMachine);
                _timer = 0;
                _idleTime = timeToRaise;
                _finishedIdle = true;
            } else {
                _stateMachine.BossReachedPlatform();
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