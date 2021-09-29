using System.Collections;
using Mechanics.Tanks;
using UnityEngine;

namespace Mechanics.Boss
{
    public class BossHealth : TankHealth
    {
        [SerializeField] private BossStateMachine _stateMachine;

        [SerializeField] [Range(0, 1)] private float _escalationHp = 0.9f;
        [SerializeField] [Range(0, 1)] private float _enragedHp = 0.25f;
        [SerializeField] [Range(0, 1)] private float _healToHp = 0.5f;
        [SerializeField] [Range(0, 1)] private float _killSequenceHp = 0.02f;

        private bool _reachedEscalation;
        private bool _reachedMidpoint;
        private bool _reachedKillSequence;

        private void Awake()
        {
            _stateMachine = GetComponentInChildren<BossStateMachine>();
        }

        public void HealEnrage(float delta)
        {
            SetHealth(Mathf.SmoothStep(_enragedHp, _healToHp, delta));
        }

        public void DamageByEnergyCell(int amount)
        {
            DecreaseHealth(amount);
        }

        protected override void OnHealthChanged()
        {
            if (!_reachedEscalation && Health <= MaxHealth * _escalationHp) {
                _stateMachine.UpdateBossStage(BossStage.Escalation);
                _reachedEscalation = true;
            }
            if (!_reachedMidpoint && Health <= MaxHealth * _enragedHp) {
                _stateMachine.UpdateBossStage(BossStage.MidpointCutscene);
                _reachedMidpoint = true;
            }
            if (!_reachedKillSequence && Health <= MaxHealth * _killSequenceHp) {
                _stateMachine.UpdateBossStage(BossStage.KillSequence);
                _reachedKillSequence = true;
            }
        }
    }
}