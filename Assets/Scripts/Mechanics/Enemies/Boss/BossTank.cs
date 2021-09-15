using System.Collections;
using Assets.Scripts.Level_Systems;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Enemies.Boss
{
    public enum BossState
    {
        Disabled,
        Locked,
        Idle,
        Moving,
        Charging
    }

    [RequireComponent(typeof(Collider))]
    public class BossTank : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector2 _idleTimeMinMax = new Vector2(5, 10);
        [SerializeField] private Vector2 _chargeTimeMinMax = new Vector2(15, 30);
        [SerializeField] private float _moveSpeed = 2;
        [Header("References")]
        [SerializeField] private GameObject _art = null;
        [SerializeField] private BossPlatform _leftPlatform = null;
        [SerializeField] private BossPlatform _centerPlatform = null;
        [SerializeField] private BossPlatform _rightPlatform = null;
        [Header("Charge State")]
        [SerializeField] private float _turnTime = 1;
        [SerializeField] private float _chargeSpeed = 2;

        private BossState _state = BossState.Idle;
        private float _timer;
        private float _idleTime;
        private float _chargeTimer;
        private float _chargeTime;
        private Collider _collider;
        private int _currentPlatform;
        private Vector3 _destination;
        private BossState _previousState;
        private bool _isCharging;
        private bool _isAlive;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (!_isAlive) return;
            _timer += Time.deltaTime;
            switch (_state) {
                case BossState.Disabled:
                    break;
                case BossState.Locked:
                    break;
                case BossState.Idle:
                    if (_timer > _idleTime) {
                        _state = BossState.Moving;
                        SetMoveDestination();
                        _timer = 0;
                    }
                    break;
                case BossState.Moving:
                    transform.position = Vector3.MoveTowards(transform.position, _destination, _moveSpeed * Time.deltaTime);
                    if (Vector3.Distance(transform.position, _destination) < 0.1f) {
                        transform.position = _destination;
                        SetIdle();
                    }
                    break;
                case BossState.Charging:
                    if (!_isCharging) {
                        _state = _previousState;
                    }
                    break;
            }
            if (_state == BossState.Charging) return;
            _chargeTimer += Time.deltaTime;
            if (_chargeTimer > _chargeTime) {
                _previousState = _state;
                _state = BossState.Charging;
                StartCoroutine(Charge());
                _chargeTime = GetChargeTime();
                _chargeTimer = 0;
            }
        }

        private IEnumerator Charge()
        {
            _isCharging = true;
            for (float t = 0; t < _turnTime; t += Time.deltaTime) {
                float delta = t / _turnTime;
                transform.rotation = Quaternion.Euler(0, 90 + delta * 90, 0);
                yield return null;
            }
            for (float t = 0; t < _chargeSpeed; t += Time.deltaTime) {
                float delta = t / _chargeSpeed;
                float zPos = 7 - delta * 12;
                Vector3 pos = transform.position;
                pos.z = zPos;
                transform.position = pos;
                yield return null;
            }
            for (float t = 0; t < _chargeSpeed; t += Time.deltaTime) {
                float delta = t / _chargeSpeed;
                float zPos = -5 + delta * 12;
                Vector3 pos = transform.position;
                pos.z = zPos;
                transform.position = pos;
                yield return null;
            }
            for (float t = 0; t < _turnTime; t += Time.deltaTime) {
                float delta = t / _turnTime;
                transform.rotation = Quaternion.Euler(0, 180 - delta * 90, 0);
                yield return null;
            }
            _isCharging = false;
        }

        private void SetIdle()
        {
            _state = BossState.Idle;
            _idleTime = GetIdleTime();
            _timer = 0;
        }

        private void SetMoveDestination()
        {
            if (_currentPlatform == 0) {
                // Middle Platform
                int rand = Random.Range(0, 100);
                if (rand > 50) {
                    _destination = _leftPlatform.GetCenter();
                    _currentPlatform = -1;
                } else {
                    _destination = _rightPlatform.GetCenter();
                    _currentPlatform = 1;
                }
            } else {
                // Left or Right Platform
                _destination = _centerPlatform.GetCenter();
                _currentPlatform = 0;
            }
        }

        public void Lock()
        {
            _state = BossState.Locked;
            _isAlive = false;
        }

        public void Unlock()
        {
            SetIdle();
            _chargeTime = GetChargeTime();
            _chargeTimer = 0;
            _isAlive = true;
        }

        public void Disable()
        {
            _state = BossState.Disabled;
            if (_art != null) _art.SetActive(false);
            _collider.enabled = false;
            _isAlive = false;
        }

        public void Enable()
        {
            if (_art != null) _art.SetActive(true);
            _collider.enabled = true;
        }

        private float GetIdleTime()
        {
            return _idleTimeMinMax.y > _idleTimeMinMax.x ? Random.Range(_idleTimeMinMax.x, _idleTimeMinMax.y) : _idleTimeMinMax.x;
        }

        private float GetChargeTime()
        {
            return _chargeTimeMinMax.y > _chargeTimeMinMax.x ? Random.Range(_chargeTimeMinMax.x, _chargeTimeMinMax.y) : _chargeTimeMinMax.x;
        }
    }
}