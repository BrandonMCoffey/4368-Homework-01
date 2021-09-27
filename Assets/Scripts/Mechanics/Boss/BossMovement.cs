using UnityEngine;

namespace Mechanics.Boss
{
    public class BossMovement : MonoBehaviour
    {
        [SerializeField] private Transform _mainTransform;
        [SerializeField] private float _movementSpeed = 2;

        public Transform MainTransform => _mainTransform;

        private void Awake()
        {
            NullTest();
        }

        public bool MoveTowards(Vector3 destination)
        {
            Vector3 newPosition = Vector3.MoveTowards(_mainTransform.position, destination, _movementSpeed * Time.deltaTime);
            _mainTransform.position = newPosition;

            return Vector3.Distance(_mainTransform.position, destination) < 0.01f;
        }

        private void NullTest()
        {
            if (_mainTransform == null) {
                _mainTransform = transform.parent;
                if (_mainTransform == null) {
                    _mainTransform = transform;
                }
            }
        }
    }
}