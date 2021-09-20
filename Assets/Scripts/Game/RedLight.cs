using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RedLight : MonoBehaviour
    {
        [SerializeField] private float _speed = 5;

        private List<RedLightHelper> _helpers = new List<RedLightHelper>();
        private float _yPos;
        private bool _enabled = false;

        private void Start()
        {
            foreach (var helper in transform.GetComponentsInChildren<RedLightHelper>()) {
                _helpers.Add(helper);
            }
        }

        private void Update()
        {
            if (!_enabled) return;
            _yPos += _speed;
            foreach (var helper in _helpers) {
                helper.Rotate(_yPos);
            }
        }

        public void SetDelta(float delta)
        {
            _enabled = delta > 0;
            foreach (var helper in _helpers) {
                helper.SetIntensityDelta(delta);
            }
        }
    }
}