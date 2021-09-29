using System.Collections.Generic;
using System.Linq;
using Level_Systems;
using Mechanics.Boss.States;
using UnityEngine;
using Utility.GameEvents.Logic;
using Utility.StateMachine;

namespace Mechanics.Boss
{
    public enum BossStage
    {
        IntroCutscene,
        Basic,
        Escalation,
        MidpointCutscene,
        Enraged,
        KillSequence
    }

    // Boss State Controller
    public class BossStateMachine : StateMachine
    {
        [SerializeField] private BossPlatformController _platformController;
        [SerializeField] private EnergyCellController _energyCellController;
        [SerializeField] private BossMovement _movement;
        [SerializeField] private BossFeedback _feedback;
        [SerializeField] private BossTurret _turret;
        [SerializeField] private GameObject _art = null;
        [SerializeField] private GameEvent _onEndCutscene = null;
        [SerializeField] private BossAiData _aiData;

        public NullState CutsceneState { get; private set; }
        public Idle IdleState { get; private set; }
        public MoveToPlatform MoveToPlatformState { get; private set; }
        public ChargeAttack ChargeAttackState { get; private set; }
        public LaserAttack LaserAttackState { get; private set; }
        public PlatformSummoning PlatformSummoningState { get; private set; }

        private BossStage _stage = BossStage.IntroCutscene;
        private List<IState> _availableAttacks = new List<IState>();
        private List<IState> _availableMovements = new List<IState>();

        public Transform MainTransform => _movement.MainTransform;

        private void Awake()
        {
            NullTest();
            CutsceneState = new NullState();
            IdleState = new Idle(this, _aiData);
            MoveToPlatformState = new MoveToPlatform(this, _platformController, _movement, _aiData);
            ChargeAttackState = new ChargeAttack(this, _movement, _aiData);
            LaserAttackState = new LaserAttack(this, _platformController, _energyCellController, _aiData);
            PlatformSummoningState = new PlatformSummoning(this);

            _availableMovements = new List<IState> { MoveToPlatformState };

            ChangeState(CutsceneState);
        }

        private void OnEnable()
        {
            if (_onEndCutscene == null) {
                OnEndCutscene();
            } else {
                _onEndCutscene.OnEvent += OnEndCutscene;
            }
        }

        private void OnDisable()
        {
            if (_onEndCutscene != null) {
                _onEndCutscene.OnEvent -= OnEndCutscene;
            }
        }

        protected override void OnStateChanged()
        {
            bool canShoot = CurrentState == IdleState || CurrentState == MoveToPlatformState;
            _turret.SetCanShoot(canShoot);
        }

        public void UpdateBossStage(BossStage stage)
        {
            if (_aiData.Debug) Debug.LogWarning("Set Stage: " + stage);
            _availableAttacks.Clear();
            _stage = stage;
            switch (_stage) {
                case BossStage.Basic:
                    _availableAttacks = new List<IState> { ChargeAttackState };
                    RandomMovementOrAttack(40, 60, 0);
                    break;
                case BossStage.Escalation:
                    _feedback.EscalationFeedback();
                    _movement.SetEscalation();
                    _platformController.SetEscalation();
                    IdleState.SetEscalation();
                    ChangeState(ChargeAttackState, true);
                    _availableAttacks = new List<IState> { ChargeAttackState, LaserAttackState, PlatformSummoningState };
                    break;
                case BossStage.MidpointCutscene:
                    _feedback.MidpointFeedback();
                    break;
                case BossStage.Enraged:
                    _availableAttacks = new List<IState> { ChargeAttackState, LaserAttackState, PlatformSummoningState };
                    break;
                case BossStage.KillSequence:
                    _feedback.KillSequenceFeedback();
                    break;
            }
            _availableAttacks = _availableAttacks.Where(item => item != null).ToList();
        }

        public void BossReachedPlatform()
        {
            if (_hasError) return;
            switch (_stage) {
                case BossStage.Basic:
                    RandomMovementOrAttack(30, 70, 0);
                    break;
                case BossStage.Escalation:
                    RandomMovementOrAttack(10, 40, 50);
                    break;
                case BossStage.Enraged:
                    RandomAttack();
                    break;
                default:
                    ChangeState(IdleState);
                    break;
            }
        }

        public void BossFinishedCharge()
        {
            if (PreviousState == MoveToPlatformState) {
                RevertToPreviousState(true, false);
            } else {
                BossFinishedAttack();
            }
        }

        public void BossFinishedAttack()
        {
            if (_hasError) return;

            if (PreviousState == MoveToPlatformState) {
                ChangeState(MoveToPlatformState);
                return;
            }

            switch (_stage) {
                case BossStage.Basic:
                    RandomMovementOrAttack(70, 30, 0);
                    break;
                case BossStage.Escalation:
                    RandomMovementOrAttack(20, 40, 40);
                    break;
                case BossStage.Enraged:
                    RandomMovementOrAttack(0, 70, 30);
                    break;
                default:
                    ChangeState(IdleState);
                    break;
            }
        }

        public void BossFinishedIdle()
        {
            if (_hasError) return;
            switch (_stage) {
                case BossStage.Basic:
                    RandomMovementOrAttack(5, 55, 40);
                    break;
                case BossStage.Escalation:
                    RandomMovementOrAttack(0, 40, 60);
                    break;
                case BossStage.Enraged:
                    RandomMovementOrAttack(0, 15, 85);
                    break;
                default:
                    ChangeState(IdleState);
                    break;
            }
        }

        private void RandomAttack()
        {
            IState attack = _availableAttacks.Count > 0 ? _availableAttacks[Random.Range(0, _availableAttacks.Count)] : IdleState;
            if (_aiData.Debug) Debug.Log("Set Attack: " + attack);
            ChangeState(attack, true);
        }

        private void RandomMovement()
        {
            IState movement = _availableMovements[Random.Range(0, _availableMovements.Count)];
            if (_aiData.Debug) Debug.Log("Set Movement: " + movement);
            ChangeState(movement);
        }

        private void RandomMovementOrAttack(int idle, int move, int attack)
        {
            int rand = Random.Range(0, idle + move + attack);
            if (rand < idle) {
                ChangeState(IdleState);
            } else if (rand < idle + move) {
                RandomMovement();
            } else {
                RandomAttack();
            }
        }

        private void OnEndCutscene()
        {
            _platformController.SetPlatform(PlatformOptions.Center);
            if (_stage == BossStage.IntroCutscene) {
                UpdateBossStage(BossStage.Basic);
            } else if (_stage == BossStage.MidpointCutscene) {
                UpdateBossStage(BossStage.Enraged);
            } else {
                Debug.Log("Warning: Incorrect State on Cutscene End - " + gameObject);
            }
        }

        public void SetVisuals(bool active)
        {
            if (_art != null) _art.SetActive(active);
        }

        #region NullTest

        private bool _hasError;

        private void NullTest()
        {
            if (_aiData == null) {
                _aiData = ScriptableObject.CreateInstance<BossAiData>();
            }
            if (_platformController == null) {
                _platformController = FindObjectOfType<BossPlatformController>();
                if (_platformController == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Boss Platform Controller for Boss AI - " + gameObject);
                }
            }
            if (_energyCellController) {
                _energyCellController = FindObjectOfType<EnergyCellController>();
                if (_energyCellController == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Energy Cell Controller for Boss AI - " + gameObject);
                }
            }
            if (_turret == null) {
                _turret = GetComponent<BossTurret>();
                if (_turret == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Turret for Boss AI - " + gameObject);
                }
            }
            if (_movement == null) {
                Transform parent = transform.parent;
                _movement = parent != null ? parent.GetComponentInChildren<BossMovement>() : GetComponent<BossMovement>();
                if (_movement == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Boss Movement Controller for Boss AI - " + gameObject);
                }
            }
            if (_feedback == null) {
                Transform parent = transform.parent != null ? transform.parent : transform;
                _feedback = parent.GetComponentInChildren<BossFeedback>();
            }
        }

        #endregion
    }
}