using Mechanics.Tanks;
using UnityEngine;

namespace Mechanics.Boss
{
    public class BossHealth : TankHealth
    {
        [SerializeField] private BossStateMachine _stateMachine;

        private bool _reachedEscalation;
        private bool _reachedMidpoint;
        private bool _reachedKillSequence;

        private void Awake()
        {
            _stateMachine = GetComponentInChildren<BossStateMachine>();
        }

        protected override void OnHealthChanged()
        {
            if (!_reachedEscalation && Health <= MaxHealth * 0.9f) {
                _stateMachine.UpdateBossStage(BossStage.Escalation);
                _reachedEscalation = true;
            }
            if (!_reachedMidpoint && Health <= MaxHealth * 0.25f) {
                _stateMachine.UpdateBossStage(BossStage.MidpointCutscene);
                _reachedMidpoint = true;
            }
            if (!_reachedKillSequence && Health <= MaxHealth * 0.02f) {
                _stateMachine.UpdateBossStage(BossStage.KillSequence);
                _reachedKillSequence = true;
            }
        }
    }
}