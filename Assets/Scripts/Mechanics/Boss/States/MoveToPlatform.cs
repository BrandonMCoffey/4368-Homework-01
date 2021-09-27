using UnityEngine;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class MoveToPlatform : IState
    {
        private BossStateMachine _bossStateMachine;
        private BossPlatformController _platformController;
        private BossMovement _bossMovement;

        public MoveToPlatform(BossStateMachine bossStateMachine, BossPlatformController platformController, BossMovement bossMovement)
        {
            _bossStateMachine = bossStateMachine;
            _platformController = platformController;
            _bossMovement = bossMovement;
        }

        private Vector3 _destination;

        public void Enter()
        {
            Transform destination = _platformController.GetNewDestination();
            if (destination == null) {
                _bossStateMachine.RevertToPreviousState(false);
                return;
            }
            _destination = destination.position;
        }

        public void Tick()
        {
            bool reachedDestination = _bossMovement.MoveTowards(_destination);

            if (reachedDestination) {
                _bossStateMachine.BossReachedPlatform();
            }
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
            _destination = default;
        }
    }
}