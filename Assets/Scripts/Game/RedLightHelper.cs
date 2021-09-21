using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Light))]
    public class RedLightHelper : MonoBehaviour
    {
        [SerializeField] private bool _rotates = true;

        private Light _light;
        private float _maxIntensity;
        private Vector3 _rotationOffset;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _maxIntensity = _light.intensity;
            _light.intensity = 0;
            _rotationOffset = transform.eulerAngles;
        }

        public void Rotate(float rot)
        {
            if (!_rotates) return;
            transform.rotation = Quaternion.Euler(_rotationOffset.x, _rotationOffset.y + rot, _rotationOffset.z);
        }

        public void SetIntensityDelta(float delta)
        {
            _light.intensity = delta * _maxIntensity;
        }
    }
}