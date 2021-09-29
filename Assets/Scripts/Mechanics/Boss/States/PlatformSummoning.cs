using Level_Systems;
using UnityEngine;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class PlatformSummoning : IState
    {
        private BossStateMachine _stateMachine;
        private BossPlatformController _platformController;
        private EnergyCellController _energyCellController;

        private float _idleTime;
        private float _timer;
        private bool _debug;
        private float _escalation = 0.5f;

        public PlatformSummoning(BossStateMachine stateMachine, BossPlatformController platformController, EnergyCellController energyCellController, BossAiData data)
        {
            _stateMachine = stateMachine;
            _platformController = platformController;
            _energyCellController = energyCellController;
            _debug = data.Debug;
        }

        public void Escalate()
        {
            _escalation = 1;
        }

        public void Enter()
        {
            if (!_platformController.IsOnPlatform) {
                if (_debug) Debug.Log("Platform Summoning: Not on Platform, Reverting...");
                _stateMachine.RevertToPreviousState();
                return;
            }
            _idleTime = _energyCellController.StartPlatformSummoning(_escalation);
            _timer = 0;
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer > _idleTime) {
                _stateMachine.BossFinishedAttack();
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