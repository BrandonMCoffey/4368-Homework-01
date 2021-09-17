using Assets.Scripts.Utility.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Boss.States
{
    public class MoveToPlatform : IState
    {
        private BossStateMachine _bossStateMachine;
        private BossPlatformController _platformController;
        private Transform _bossTransform;

        private float _speed = 10;

        public MoveToPlatform(BossStateMachine bossStateMachine, BossPlatformController platformController, Transform bossTransform)
        {
            _bossStateMachine = bossStateMachine;
            _platformController = platformController;
            _bossTransform = bossTransform;
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
            Vector3 newPosition = Vector3.MoveTowards(_bossTransform.position, _destination, _speed * Time.deltaTime);
            _bossTransform.position = newPosition;

            if (Vector3.Distance(_bossTransform.position, _destination) < 0.01f) {
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