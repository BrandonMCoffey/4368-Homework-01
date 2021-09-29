using System;
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
        private Coroutine _chargeRoutine = null;
        private bool _isRotated;
        private bool _isRetreating;

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

        public void Escalate()
        {
            _timeToRotate /= 1.5f;
        }

        public void Enter()
        {
            _velocity = 0;
            if (_chargeRoutine != null) {
                _stateMachine.StopCoroutine(_chargeRoutine);
            }
            if (_isRotated) {
                if (_isRetreating) _bossMovement.StartSecondCharge();
                NextEvent(Charge());
            } else {
                _bossMovement.StartCharge();
                NextEvent(Rotate());
            }
        }

        private void NextEvent(IEnumerator nextEvent)
        {
            _chargeRoutine = _stateMachine.StartCoroutine(nextEvent);
        }

        private IEnumerator Rotate()
        {
            //if (_debug) Debug.Log("ChargeAttack: Rotate");
            for (float t = 0; t < _timeToRotate; t += Time.deltaTime) {
                float delta = t / _timeToRotate;
                _bossMovement.Rotate(90f * delta);
                yield return null;
            }
            _isRotated = true;
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
            //if (_debug) Debug.Log("ChargeAttack: Impact");
            _bossMovement.Impact();
            yield return new WaitForSecondsRealtime(_impactHold);
            NextEvent(Retreat());
        }

        private IEnumerator Retreat()
        {
            _isRetreating = true;
            if (_debug) Debug.Log("ChargeAttack: Retreat");
            while (true) {
                bool finished = _bossMovement.Retreat(_retreatSpeed * Time.deltaTime);
                if (finished) {
                    break;
                }
                yield return null;
            }
            _isRetreating = false;
            NextEvent(EndRotate());
        }

        private IEnumerator EndRotate()
        {
            //if (_debug) Debug.Log("ChargeAttack: EndRotate");
            for (float t = 0; t < _timeToRotate; t += Time.deltaTime) {
                float delta = t / _timeToRotate;
                _bossMovement.Rotate(90f - 90f * delta);
                yield return null;
            }
            _isRotated = false;
            _stateMachine.BossFinishedCharge();
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
            if (_chargeRoutine != null) {
                _stateMachine.StopCoroutine(_chargeRoutine);
            }
        }
    }
}