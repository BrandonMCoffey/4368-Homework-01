using System.Collections;
using Interfaces;
using UnityEngine;

namespace Level_Systems
{
    public class PlatformSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _timeToLower = 1;
        [SerializeField] private float _timeToRaise = 1;
        [SerializeField] private Vector3 _lowerOffset = new Vector3(0, -1, 0);
        [Header("Obstacle Checks")]
        [SerializeField] private float _checkHeight = 0.5f;
        [SerializeField] private float _checkRadius = 1;
        [SerializeField] private LayerMask _checkLayer = 1;
        [Header("References")]
        [SerializeField] private Collider _spawningCollider = null;
        [SerializeField] private Transform _platformToMove = null;

        private Vector3 _startPos;
        private Vector3 _endPos;

        private Coroutine _routine = null;
        private GameObject _objectToSpawn;

        private void OnEnable()
        {
            _startPos = _platformToMove.position;
            _endPos = _platformToMove.position + _lowerOffset;
        }

        public bool IsClear()
        {
            return _routine == null && !Physics.CheckSphere(transform.position + Vector3.up * _checkHeight, _checkRadius, _checkLayer);
        }

        public void PrepareToSpawn(GameObject spawnableObject)
        {
            if (_platformToMove == null) return;
            if (_spawningCollider != null) {
                _spawningCollider.enabled = true;
            }
            _objectToSpawn = spawnableObject;
            _routine = StartCoroutine(Lower());
        }

        private IEnumerator Lower()
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
            GameObject obj = Instantiate(_objectToSpawn, _endPos, Quaternion.identity);
            LockObject(obj);
            _routine = StartCoroutine(Raise(obj));
        }

        private IEnumerator Raise(GameObject objToRaise)
        {
            for (float t = 0; t < _timeToRaise; t += Time.deltaTime) {
                float delta = t / _timeToRaise;
                Vector3 pos = Vector3.Lerp(_endPos, _startPos, delta);
                _platformToMove.position = pos;
                objToRaise.transform.position = pos;
                yield return null;
            }
            _platformToMove.position = _startPos;
            Finish(objToRaise);
        }

        private void Finish(GameObject obj)
        {
            _routine = null;
            if (_spawningCollider != null) {
                _spawningCollider.enabled = false;
            }
            LockObject(obj, false);
        }

        private void LockObject(GameObject obj, bool active = true)
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