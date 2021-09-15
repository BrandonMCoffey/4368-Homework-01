using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemies.Boss;
using Assets.Scripts.Level_Systems;
using Assets.Scripts.Player_Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public enum IntroState
    {
        FadeIn,
        PlayerMoveIn,
        EnableLights,
        BossEnter,
        Pause,
        DimLights,
        StartGame
    }

    public class BossIntroSequence : MonoBehaviour
    {
        [SerializeField] private bool _skipCutscene = false;

        [Header("Time of Events")]
        [SerializeField] private float _fadeIn = 1;
        [SerializeField] private float _playerEnter = 1;
        [SerializeField] private float _brightenLights = 1;
        [SerializeField] private float _pauseTime = 1;
        [SerializeField] private float _dimLights = 1;
        [Header("Temporary Separate Player Tank Art")]
        [SerializeField] private Transform _tempPlayerArt = null;
        [SerializeField] private Vector3 _artStartPos = Vector3.zero;
        [SerializeField] private Vector3 _artEndPos = Vector3.zero;
        [Header("Lights")]
        [SerializeField] private Light _directionalLight = null;
        [SerializeField] private float _directionalMin = 0.25f;
        [SerializeField] private float _directionalMax = 1;
        [SerializeField] private List<RedLight> _redLights = new List<RedLight>();
        [SerializeField] private List<RedLightHelper> _backingRedLights = new List<RedLightHelper>();
        [Header("References")]
        [SerializeField] private PlayerTank _player;
        [SerializeField] private BossTank _boss;
        [SerializeField] private BossPlatform _bossSpawnPlatform = null;
        [SerializeField] private Image _fadeInPanel = null;

        private IntroState _state = IntroState.FadeIn;
        private float _bossEnterTime;
        private float _timer;
        private bool _finished;
        private bool _hasError;

        private void Start()
        {
            if (_player == null) {
                _player = FindObjectOfType<PlayerTank>();
                if (_player == null) {
                    _hasError = true;
                    throw new MissingComponentException("No assigned Player Tank on " + gameObject);
                }
            }
            if (_boss == null) {
                _boss = FindObjectOfType<BossTank>();
                if (_boss == null) {
                    _hasError = true;
                    throw new MissingComponentException("No assigned Boss Tank on " + gameObject);
                }
            }
            if (_bossSpawnPlatform != null) {
                _bossEnterTime = _bossSpawnPlatform.TotalTime;
            } else {
                _hasError = true;
                throw new MissingComponentException("No assigned Boss Tank Spawn Platform on " + gameObject);
            }

            if (_skipCutscene) {
                _directionalLight.intensity = _directionalMax;
                foreach (var redLight in _redLights.Where(redLight => redLight != null)) {
                    redLight.gameObject.SetActive(false);
                }
                foreach (var backRedLight in _backingRedLights.Where(backRedLight => backRedLight != null)) {
                    backRedLight.gameObject.SetActive(true);
                    backRedLight.SetIntensityDelta(1);
                }
                return;
            }
            _boss.Disable();
            _player.gameObject.SetActive(false);
            _tempPlayerArt.gameObject.SetActive(true);
            _fadeInPanel.gameObject.SetActive(true);
            _directionalLight.intensity = _directionalMin;
            foreach (var redLight in _redLights.Where(redLight => redLight != null)) {
                redLight.gameObject.SetActive(true);
                redLight.SetDelta(0);
            }
            foreach (var backRedLight in _backingRedLights.Where(backRedLight => backRedLight != null)) {
                backRedLight.gameObject.SetActive(true);
                backRedLight.SetIntensityDelta(0);
            }
            _timer = 0;
        }

        private void Update()
        {
            if (_hasError || _skipCutscene || _finished) return;

            _timer += Time.deltaTime;

            switch (_state) {
                case IntroState.FadeIn:
                    Color col = _fadeInPanel.color;
                    col.a = 1 - _timer / _fadeIn;
                    _fadeInPanel.color = col;
                    if (_timer > _fadeIn) {
                        _fadeInPanel.gameObject.SetActive(false);
                        _state = IntroState.PlayerMoveIn;
                        _timer = 0;
                    }
                    break;
                case IntroState.PlayerMoveIn:
                    _tempPlayerArt.position = Vector3.Slerp(_artStartPos, _artEndPos, _timer / _playerEnter);
                    if (_timer > _playerEnter) {
                        _tempPlayerArt.position = _artEndPos;
                        _state = IntroState.EnableLights;
                        _timer = 0;
                    }
                    break;
                case IntroState.EnableLights:
                    foreach (var redLight in _redLights.Where(redLight => redLight != null)) {
                        redLight.SetDelta(_timer / _brightenLights);
                    }
                    if (_timer > _playerEnter) {
                        foreach (var redLight in _redLights.Where(redLight => redLight != null)) {
                            redLight.SetDelta(1);
                        }
                        _state = IntroState.BossEnter;
                        _bossSpawnPlatform.PrepareToRaise(_boss);
                        _timer = 0;
                    }
                    break;
                case IntroState.BossEnter:
                    if (_timer > _bossEnterTime) {
                        _state = IntroState.Pause;
                        _timer = 0;
                    }
                    break;
                case IntroState.Pause:
                    if (_timer > _pauseTime) {
                        _state = IntroState.DimLights;
                        _timer = 0;
                    }
                    break;
                case IntroState.DimLights:
                    foreach (var redLight in _redLights.Where(redLight => redLight != null)) {
                        redLight.SetDelta(1 - _timer / _brightenLights);
                    }
                    foreach (var backRedLight in _backingRedLights.Where(backRedLight => backRedLight != null)) {
                        backRedLight.SetIntensityDelta(_timer / _brightenLights);
                    }
                    _directionalLight.intensity = _directionalMax - (_directionalMax - _directionalMin) * (1 - _timer / _brightenLights);
                    if (_timer > _dimLights) {
                        foreach (var redLight in _redLights.Where(redLight => redLight != null)) {
                            redLight.gameObject.SetActive(false);
                        }
                        foreach (var backRedLight in _backingRedLights.Where(backRedLight => backRedLight != null)) {
                            backRedLight.SetIntensityDelta(1);
                        }
                        _state = IntroState.StartGame;
                        _timer = 0;
                    }
                    break;
                case IntroState.StartGame:
                    _player.transform.position = _artEndPos;
                    _player.gameObject.SetActive(true);
                    _tempPlayerArt.gameObject.SetActive(false);
                    _finished = true;
                    break;
            }
        }
    }
}