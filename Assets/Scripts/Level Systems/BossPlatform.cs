using System.Collections;
using Assets.Scripts.Enemies.Boss;
using Assets.Scripts.Interfaces;
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

        private BossTank _bossTank;
        private bool _lower;

        public float TotalTime => _timeToLower + _timeToRaise;

        private void OnEnable()
        {
            _startPos = _platformToMove.position;
            _endPos = _platformToMove.position + _lowerOffset;
        }

        public void PrepareToLower(BossTank bossTank)
        {
            if (_platformToMove == null) return;
            _lower = true;
            _bossTank = bossTank;
            _bossTank.Lock();
            StartCoroutine(Lower(_bossTank.transform));
        }

        public void PrepareToRaise(BossTank bossTank)
        {
            if (_platformToMove == null) return;
            _lower = false;
            _bossTank = bossTank;
            _bossTank.Disable();
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
                _bossTank.Disable();
                StartCoroutine(Raise(null));
            } else {
                _bossTank.Enable();
                _bossTank.Lock();
                StartCoroutine(Raise(_bossTank.transform));
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
            _bossTank.Unlock();
        }

        private void LockObject(BossTank obj, bool active = true)
        {
            ILockable lockableObject = obj.GetComponent<ILockable>();
            if (lockableObject != null) {
                lockableObject.Lock(active);
            } else {
                Collider col = obj.GetComponent<Collider>();
                if (col != null) {
                    col.enabled = !active;
                }
            }
        }
    }
}