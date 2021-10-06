using System.Collections.Generic;
using System.Linq;
using Game;
using Level_Systems;
using Mechanics.Boss.States;
using Mechanics.Projectiles;
using Mechanics.Tanks;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private MidpointCutscene _midpointCutscene;
        [SerializeField] private BossMovement _movement;
        [SerializeField] private BossFeedback _feedback;
        [SerializeField] private BossTurret _turret;
        [SerializeField] private GameObject _art = null;
        [SerializeField] private GameEvent _onEndCutscene = null;
        [SerializeField] private BossAiData _aiData;
        [SerializeField] private TankFire _fire = null;
        [SerializeField] private TankAim _aim = null;
        [Header("Colors")]
        [SerializeField] private Image _colorToSet = null;
        [SerializeField] private Color _basicColor = Color.white;
        [SerializeField] private Color _escalationColor = Color.magenta;
        [SerializeField] private Color _enragedColor = Color.red;
        [SerializeField] private Color _killColor = Color.black;

        public NullState CutsceneState { get; private set; }
        public Idle IdleState { get; private set; }
        public MoveToPlatform MoveToPlatformState { get; private set; }
        public ChargeAttack ChargeAttackState { get; private set; }
        public LaserAttack LaserAttackState { get; private set; }
        public PlatformSummoning PlatformSummoningState { get; private set; }

        private BossStage _stage = BossStage.IntroCutscene;
        private List<IState> _availableAttacks = new List<IState>();
        private List<IState> _availableMovements = new List<IState>();
        private IState _previousAttack;

        public Transform MainTransform => _movement.MainTransform;

        private bool _readyMidpointCutscene;

        private void Awake()
        {
            NullTest();
            CutsceneState = new NullState();
            IdleState = new Idle(this, _aiData);
            MoveToPlatformState = new MoveToPlatform(this, _platformController, _movement, _aiData);
            ChargeAttackState = new ChargeAttack(this, _movement, _aiData);
            LaserAttackState = new LaserAttack(this, _platformController, _energyCellController, _aiData);
            PlatformSummoningState = new PlatformSummoning(this, _platformController, _energyCellController, _aiData);

            _availableMovements = new List<IState> { MoveToPlatformState };

            ChangeState(CutsceneState);
        }

        private void Start()
        {
            if (_onEndCutscene == null) {
                OnEndCutscene();
            }
        }

        private void OnEnable()
        {
            if (_onEndCutscene != null) {
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

        public void ReadyMidpointCutscene()
        {
            _readyMidpointCutscene = true;
        }

        public void UpdateBossStage(BossStage stage)
        {
            if (_aiData.Debug) Debug.LogWarning("<color=red>Set Stage: </color>" + stage);
            _availableAttacks.Clear();
            _stage = stage;
            switch (_stage) {
                case BossStage.Basic:
                    if (_colorToSet != null) _colorToSet.color = _basicColor;
                    _availableAttacks = new List<IState> { ChargeAttackState };
                    RandomMovementOrAttack(40, 60, 0);
                    break;
                case BossStage.Escalation:
                    if (_colorToSet != null) _colorToSet.color = _escalationColor;
                    _feedback.EscalationFeedback();
                    _movement.Escalate();
                    _aim.Escalate();
                    _turret.Escalate();
                    _platformController.Escalate();
                    IdleState.Escalate();
                    ChargeAttackState.Escalate();
                    if (_aiData.Debug) Debug.Log("<color=orange>Set Movement: </color>" + ChargeAttackState.GetType().Name);
                    ChangeState(ChargeAttackState, true);
                    _availableAttacks = new List<IState> { ChargeAttackState, LaserAttackState, PlatformSummoningState };
                    break;
                case BossStage.MidpointCutscene:
                    _midpointCutscene.StartCutscene();
                    _readyMidpointCutscene = false;
                    InTransition = true;
                    break;
                case BossStage.Enraged:
                    if (_colorToSet != null) _colorToSet.color = _enragedColor;
                    _feedback.MidpointFeedback();
                    if (_fire != null) _fire.SetBulletType(BulletType.Fast);
                    _movement.Escalate();
                    _aim.Escalate();
                    _turret.Escalate();
                    _platformController.Escalate();
                    IdleState.Escalate();
                    PlatformSummoningState.Escalate();
                    ChargeAttackState.Escalate();
                    _availableAttacks = new List<IState> { ChargeAttackState, LaserAttackState, PlatformSummoningState };
                    InTransition = false;
                    RandomAttack();
                    break;
                case BossStage.KillSequence:
                    if (_colorToSet != null) _colorToSet.color = _killColor;
                    _feedback.KillSequenceFeedback();
                    ChangeState(CutsceneState);
                    break;
            }
            _availableAttacks = _availableAttacks.Where(item => item != null).ToList();
        }

        public void BossReachedPlatform()
        {
            if (_hasError) return;
            PreviousState = IdleState;
            if (_readyMidpointCutscene) {
                UpdateBossStage(BossStage.MidpointCutscene);
                return;
            }
            _platformController.EnsureCurrentPlatform(this);
            switch (_stage) {
                case BossStage.Basic:
                    RandomMovementOrAttack(85, 0, 15);
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
            if (_readyMidpointCutscene) {
                UpdateBossStage(BossStage.MidpointCutscene);
                return;
            }
            if (PreviousState == MoveToPlatformState) {
                RevertToPreviousState(true, false);
            } else {
                BossFinishedAttack();
            }
        }

        public void BossFinishedAttack()
        {
            if (_readyMidpointCutscene) {
                UpdateBossStage(BossStage.MidpointCutscene);
                return;
            }
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
            if (_readyMidpointCutscene) {
                UpdateBossStage(BossStage.MidpointCutscene);
                return;
            }
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
            if (!_availableAttacks.Contains(_previousAttack)) {
                _availableAttacks.Add(_previousAttack);
            }
            IState attack = _availableAttacks.Count > 0 ? _availableAttacks[Random.Range(0, _availableAttacks.Count)] : IdleState;
            if (attack != null && _aiData.Debug) Debug.Log("<color=orange>Set Attack: </color>" + attack.GetType().Name);
            _availableAttacks.Remove(attack);
            _previousAttack = attack;
            ChangeState(attack, true);
        }

        private void RandomMovement()
        {
            IState movement = _availableMovements[Random.Range(0, _availableMovements.Count)];
            if (_aiData.Debug) Debug.Log("<color=aqua>Set Movement: </color>" + movement.GetType().Name);
            ChangeState(movement);
        }

        private void RandomMovementOrAttack(int idle, int move, int attack)
        {
            int rand = Random.Range(0, idle + move + attack);
            if (rand < idle) {
                if (_aiData.Debug) Debug.Log("<color=yellow>Set Idle: </color>" + IdleState.GetType().Name);
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
            if (_midpointCutscene == null) {
                _midpointCutscene = FindObjectOfType<MidpointCutscene>();
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