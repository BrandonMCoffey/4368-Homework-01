using System.Collections;
using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private Transform _zoomInTransform = null;

        private Vector3 _mainPosition;

        private void Awake()
        {
            if (_mainCamera == null) {
                _mainCamera = GetComponentInChildren<Camera>().transform;
            }
            _mainPosition = _mainCamera.position;
        }

        public void ShakeCamera(float duration, float intensity)
        {
            StartCoroutine(Shake(duration, intensity));
        }

        private IEnumerator Shake(float duration, float intensity)
        {
            Vector3 originalPosition = _mainCamera.localPosition;
            for (float t = 0; t < duration; t += Time.deltaTime) {
                float delta = 1 - Mathf.Abs(1f - 2 * t / duration);
                _mainCamera.localPosition = originalPosition + Random.insideUnitSphere * intensity * delta;
                yield return null;
            }
        }

        public void Zoom(float delta)
        {
            _mainCamera.position = Vector3.Slerp(_mainPosition, _zoomInTransform.position, delta);
        }
    }
}