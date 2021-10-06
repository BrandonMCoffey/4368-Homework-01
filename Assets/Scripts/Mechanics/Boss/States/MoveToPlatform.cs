using UnityEngine;
using Utility.CustomFloats;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class MoveToPlatform : IState
    {
        private BossStateMachine _bossStateMachine;
        private BossPlatformController _platformController;
        private BossMovement _bossMovement;
        private bool _willChargeDuringMovement;
        private float _changeToCharge;
        private Vector2 _whenToChargeMinMax;
        private float _whenToCharge;
        private float _chargeTimer;

        public MoveToPlatform(BossStateMachine bossStateMachine, BossPlatformController platformController, BossMovement bossMovement, BossAiData data)
        {
            _bossStateMachine = bossStateMachine;
            _platformController = platformController;
            _bossMovement = bossMovement;
            _changeToCharge = data.ChangeToCharge;
            _whenToChargeMinMax = data.WhenToCharge;
        }

        private Vector3 _destination;

        public void Escalate()
        {
            _whenToChargeMinMax /= 2;
        }

        public void Enter()
        {
            Transform destination = _platformController.GetNewDestination();
            if (destination == null) {
                Debug.Log("  - Move To Platform: Invalid Platform");
                _bossStateMachine.RevertToPreviousState(false);
                return;
            }

            Debug.Log("  - Destination: " + destination.gameObject.name, destination.gameObject);
            _destination = destination.position;

            int rand = Random.Range(0, 100);
            if (rand <= _changeToCharge) {
                _willChargeDuringMovement = true;
                _whenToCharge = RandomFloat.MinMax(_whenToChargeMinMax);
                Debug.Log("  - After " + _whenToCharge.ToString("F2") + "s: Set to ChargeAttack");
                _chargeTimer = 0;
            } else {
                _willChargeDuringMovement = false;
            }
        }

        public void Tick()
        {
            bool reachedDestination = _bossMovement.MoveTowards(_destination);

            if (reachedDestination) {
                _bossStateMachine.BossReachedPlatform();
                _willChargeDuringMovement = false;
                return;
            }
            if (_willChargeDuringMovement) {
                _chargeTimer += Time.deltaTime;
                if (_chargeTimer > _whenToCharge) {
                    Debug.Log("<color=orange>ChargeAttack</color>");
                    _bossStateMachine.ChangeState(_bossStateMachine.ChargeAttackState);
                    _willChargeDuringMovement = false;
                }
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