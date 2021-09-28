using System.Collections;
using UnityEngine;
using Utility.StateMachine;

namespace Mechanics.Boss.States
{
    public class ChargeAttack : IState
    {
        private BossStateMachine _stateMachine;
        private BossMovement _bossMovement;
        private float _timeToRotate;
        private float _acceleration;
        private float _velocity;
        private float _retreatSpeed;
        private float _impactHold;
        private bool _debug;

        public ChargeAttack(BossStateMachine stateMachine, BossMovement bossMovement, BossAiData data)
        {
            _stateMachine = stateMachine;
            _bossMovement = bossMovement;
            _timeToRotate = data.TimeToRotate;
            _acceleration = data.ChargeAcceleration;
            _retreatSpeed = data.RetreatSpeed;
            _impactHold = data.ImpactHoldTime;
            _debug = data.Debug;
        }

        public void Enter()
        {
            _velocity = 0;
            _bossMovement.StartCharge();
            NextEvent(Rotate());
        }

        private void NextEvent(IEnumerator nextEvent)
        {
            _stateMachine.StartCoroutine(nextEvent);
        }

        private IEnumerator Rotate()
        {
            if (_debug) Debug.Log("ChargeAttack: Rotate");
            for (float t = 0; t < _timeToRotate; t += Time.deltaTime) {
                float delta = t / _timeToRotate;
                _bossMovement.Rotate(90f * delta);
                yield return null;
            }
            NextEvent(Charge());
        }

        private IEnumerator Charge()
        {
            if (_debug) Debug.Log("ChargeAttack: Charge");
            while (true) {
                _velocity += _acceleration * Time.deltaTime;
                bool finished = _bossMovement.Charge(_velocity * Time.deltaTime);
                if (finished) {
                    break;
                }
                yield return null;
            }
            NextEvent(Impact());
        }

        private IEnumerator Impact()
        {
            if (_debug) Debug.Log("ChargeAttack: Impact");
            _bossMovement.Impact();
            yield return new WaitForSecondsRealtime(_impactHold);
            NextEvent(Retreat());
        }

        private IEnumerator Retreat()
        {
            if (_debug) Debug.Log("ChargeAttack: Retreat");
            while (true) {
                bool finished = _bossMovement.Retreat(_retreatSpeed * Time.deltaTime);
                if (finished) {
                    break;
                }
                yield return null;
            }
            NextEvent(EndRotate());
        }

        private IEnumerator EndRotate()
        {
            if (_debug) Debug.Log("ChargeAttack: EndRotate");
            for (float t = 0; t < _timeToRotate; t += Time.deltaTime) {
                float delta = t / _timeToRotate;
                _bossMovement.Rotate(90f - 90f * delta);
                yield return null;
            }
            _stateMachine.BossFinishedAttack();
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
        }
    }
}