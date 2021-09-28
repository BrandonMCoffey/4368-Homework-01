using Mechanics.Tanks;
using UnityEngine;

namespace Mechanics.Boss
{
    public class BossHealth : TankHealth
    {
        [SerializeField] private BossStateMachine _stateMachine;

        [SerializeField] [Range(0, 1)] private float _escalationHp = 0.9f;
        [SerializeField] [Range(0, 1)] private float _enragedHP = 0.25f;
        [SerializeField] [Range(0, 1)] private float _killSequenceHP = 0.02f;

        private bool _reachedEscalation;
        private bool _reachedMidpoint;
        private bool _reachedKillSequence;

        private void Awake()
        {
            _stateMachine = GetComponentInChildren<BossStateMachine>();
        }

        protected override void OnHealthChanged()
        {
            if (!_reachedEscalation && Health <= MaxHealth * _escalationHp) {
                _stateMachine.UpdateBossStage(BossStage.Escalation);
                _reachedEscalation = true;
            }
            if (!_reachedMidpoint && Health <= MaxHealth * _enragedHP) {
                _stateMachine.UpdateBossStage(BossStage.MidpointCutscene);
                _reachedMidpoint = true;
            }
            if (!_reachedKillSequence && Health <= MaxHealth * _killSequenceHP) {
                _stateMachine.UpdateBossStage(BossStage.KillSequence);
                _reachedKillSequence = true;
            }
        }
    }
}