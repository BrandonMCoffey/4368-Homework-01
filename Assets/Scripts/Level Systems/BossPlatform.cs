using System.Collections;
using Mechanics.Boss;
using UnityEngine;

namespace Level_Systems
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
        private float _timeMultiplier = 1;

        public float LowerTimer => _timeToLower * _timeMultiplier;
        public float RaiseTime => _timeToRaise * _timeMultiplier;
        public float TotalTime => LowerTimer + RaiseTime;

        private void OnEnable()
        {
            _startPos = _platformToMove.position;
            _endPos = _platformToMove.position + _lowerOffset;
        }

        public void Escalate()
        {
            _timeMultiplier *= 0.5f;
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
            float timeToLower = _timeToLower * _timeMultiplier;
            for (float t = 0; t < timeToLower; t += Time.deltaTime) {
                float delta = t / timeToLower;
                Vector3 pos = Vector3.Lerp(_startPos, _endPos, delta);
                _platformToMove.position = pos;
                if (bossTransform != null) {
                    bossTransform.position = pos;
                }
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
            float timeToRaise = _timeToRaise * _timeMultiplier;
            for (float t = 0; t < timeToRaise; t += Time.deltaTime) {
                float delta = t / timeToRaise;
                Vector3 pos = Vector3.Lerp(_endPos, _startPos, delta);
                _platformToMove.position = pos;
                if (bossTransform != null) {
                    bossTransform.position = pos;
                }
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