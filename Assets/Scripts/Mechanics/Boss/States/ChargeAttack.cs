using Assets.Scripts.Utility.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Boss.States
{
    public class ChargeAttack : IState
    {
        private BossStateMachine _stateMachine;
        private Transform _bossTransform;

        public ChargeAttack(BossStateMachine stateMachine, Transform bossTransform)
        {
            _stateMachine = stateMachine;
            _bossTransform = bossTransform;
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void FixedTick()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}