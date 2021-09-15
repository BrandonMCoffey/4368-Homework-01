using System;
using Assets.Scripts.Level_Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemies.Boss
{
    public enum BossState
    {
        Disabled,
        Locked,
        Idle,
        Moving
    }

    [RequireComponent(typeof(Collider))]
    public class BossTank : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector2 _idleTimeMinMax = new Vector2(2, 6);
        [Header("References")]
        [SerializeField] private GameObject _art = null;
        [SerializeField] private BossPlatform _leftPlatform = null;
        [SerializeField] private BossPlatform _centerPlatform = null;
        [SerializeField] private BossPlatform _rightPlatform = null;

        private BossState _state = BossState.Idle;
        private float _timer;
        private float _idleTime;
        private bool _doneMoving = false;
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Update()
        {
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
                    }
                    break;
                case BossState.Moving:
                    if (_doneMoving) {
                        _state = BossState.Idle;
                        _idleTime = GetIdleTime();
                    }
                    break;
            }
        }

        private void SetMoveDestination()
        {
        }

        public void Lock()
        {
            _state = BossState.Locked;
        }

        public void Unlock()
        {
            _state = BossState.Idle;
            _idleTime = GetIdleTime();
        }

        public void Disable()
        {
            _state = BossState.Disabled;
            if (_art != null) _art.SetActive(false);
            _collider.enabled = false;
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
    }
}