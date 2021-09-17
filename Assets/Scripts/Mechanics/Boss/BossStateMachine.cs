using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;
using Assets.Scripts.Mechanics.Boss.States;
using Assets.Scripts.Utility.GameEvents.Logic;
using Assets.Scripts.Utility.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Boss
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
        [SerializeField] private Transform _mainTransform;
        [SerializeField] private GameObject _art = null;
        [SerializeField] private GameEvent _onEndCutscene = null;

        public NullState CutsceneState { get; private set; }
        public Idle NotInArenaState { get; private set; }
        public Idle IdleState { get; private set; }
        public MoveToPlatform MoveToPlatformState { get; private set; }
        public ChargeAttack ChargeAttackState { get; private set; }
        public LaserAttack LaserAttackState { get; private set; }
        public PlatformSummoning PlatformSummoningState { get; private set; }

        private BossStage _stage = BossStage.IntroCutscene;
        private List<IState> _availableAttacks = new List<IState>();
        private List<IState> _availableMovements = new List<IState>();

        public Transform MainTransform => _mainTransform;

        #region NullTest

        private bool _hasError;

        private void NullTest()
        {
            if (_platformController == null) {
                _platformController = FindObjectOfType<BossPlatformController>();
                if (_platformController == null) {
                    _hasError = true;
                    throw new MissingComponentException("Missing Boss Platform Controller for Boss AI - " + gameObject);
                }
            }
            if (_mainTransform == null) {
                _mainTransform = transform.parent;
                if (_mainTransform == null) {
                    _mainTransform = transform;
                }
            }
        }

        #endregion

        private void Awake()
        {
            NullTest();
            CutsceneState = new NullState();
            NotInArenaState = new Idle(this);
            IdleState = new Idle(this);
            MoveToPlatformState = new MoveToPlatform(this, _platformController, _mainTransform);
            ChargeAttackState = new ChargeAttack(this, _mainTransform);
            LaserAttackState = new LaserAttack(this);
            PlatformSummoningState = new PlatformSummoning(this);

            _availableMovements = new List<IState> { IdleState, MoveToPlatformState };

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

        public void UpdateBossStage(BossStage stage)
        {
            _availableAttacks.Clear();
            _stage = stage;
            switch (_stage) {
                case BossStage.Basic:
                    _availableAttacks = new List<IState> { ChargeAttackState };
                    ChangeState(MoveToPlatformState);
                    break;
                case BossStage.Escalation:
                case BossStage.Enraged:
                    _availableAttacks = new List<IState> { ChargeAttackState, LaserAttackState, PlatformSummoningState };
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

        public void BossFinishedAttack()
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
                    RandomMovementOrAttack(15, 35, 50);
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
            ChangeState(_availableAttacks.Count > 0 ? _availableAttacks[Random.Range(0, _availableAttacks.Count)] : IdleState);
        }

        private void RandomMovement()
        {
            ChangeState(_availableMovements[Random.Range(0, _availableMovements.Count)]);
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
    }
}