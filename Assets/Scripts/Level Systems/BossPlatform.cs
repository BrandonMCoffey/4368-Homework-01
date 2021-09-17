using System.Collections;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Mechanics.Boss;
using UnityEngine;

namespace Assets.Scripts.Level_Systems
{
    public class BossPlatform : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _timeToLower = 1;
        [SerializeField] private float _timeToRaise = 1;
        [SerializeField] private Vector3 _lowerOffset = new Vector3(0, -1, 0);
        [Header("References")]
        [SerializeField] private Transform _platformToMove = null;

        private Vector3 _startPos;
        private Vector3 _endPos;

        private BossStateMachine _bossStateMachine;
        private bool _lower;

        public float TotalTime => _timeToLower + _timeToRaise;

        private void OnEnable()
        {
            _startPos = _platformToMove.position;
            _endPos = _platformToMove.position + _lowerOffset;
        }

        public Vector3 GetCenter()
        {
            return transform.position;
        }

        public void PrepareToLower(BossStateMachine bossTank)
        {
            if (_platformToMove == null) return;
            _lower = true;
            _bossStateMachine = bossTank;
            StartCoroutine(Lower(_bossStateMachine.MainTransform));
        }

        public void PrepareToRaise(BossStateMachine bossTank)
        {
            if (_platformToMove == null) return;
            _lower = false;
            _bossStateMachine = bossTank;
            StartCoroutine(Lower(null));
        }

        private IEnumerator Lower(Transform bossTransform)
        {
            for (float t = 0; t < _timeToLower; t += Time.deltaTime) {
                float delta = t / _timeToLower;
                _platformToMove.position = Vector3.Lerp(_startPos, _endPos, delta);
                yield return null;
            }
            _platformToMove.position = _endPos;
            Spawn();
        }

        private void Spawn()
        {
            if (_lower) {
                _bossStateMachine.SetVisuals(false);
                StartCoroutine(Raise(null));
            } else {
                _bossStateMachine.SetVisuals(true);
                StartCoroutine(Raise(_bossStateMachine.MainTransform));
            }
        }

        private IEnumerator Raise(Transform bossTransform)
        {
            for (float t = 0; t < _timeToRaise; t += Time.deltaTime) {
                float delta = t / _timeToRaise;
                Vector3 pos = Vector3.Lerp(_endPos, _startPos, delta);
                _platformToMove.position = pos;
                bossTransform.position = pos;
                yield return null;
            }
            _platformToMove.position = _startPos;
            Finish();
        }

        private void Finish()
        {
            if (!_lower) {
                _bossStateMachine.BossReachedPlatform();
            }
        }
    }
}