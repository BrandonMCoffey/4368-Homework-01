using System.Collections;
using Level_Systems;
using UnityEngine;
using Utility.CustomFloats;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class LaserAttack : IState
    {
        private BossStateMachine _stateMachine;
        private BossPlatformController _platformController;
        private EnergyCellController _energyCellController;

        private float _idleTime;
        private float _timer;
        private bool _finishedIdle;
        private bool _debug;

        public LaserAttack(BossStateMachine bossStateMachine, BossPlatformController platformController, EnergyCellController energyCellController, BossAiData data)
        {
            _stateMachine = bossStateMachine;
            _platformController = platformController;
            _energyCellController = energyCellController;
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
            _idleTime = _energyCellController.BeginLaserAttack(timeToLower / 2);

            _timer = -timeToLower / 4;
            if (_debug) Debug.Log("Laser Attack: Idle for " + _idleTime);
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