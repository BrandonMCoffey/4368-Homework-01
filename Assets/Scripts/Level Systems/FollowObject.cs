using UnityEngine;

namespace Level_Systems
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform _objectToFollow;

        private Vector3 _offset;

        private void Start()
        {
            _offset = transform.position - _objectToFollow.position;
        }

        private void Update()
        {
            transform.position = _objectToFollow.position + _offset;
        }
    }
}