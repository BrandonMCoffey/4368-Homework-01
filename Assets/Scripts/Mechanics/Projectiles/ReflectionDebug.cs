using UnityEngine;

namespace Assets.Scripts.Mechanics.Projectiles
{
    public class ReflectionDebug : MonoBehaviour
    {
        [SerializeField] private bool _active = false;
        [SerializeField] private int _reflectionTimes = 10;
        [SerializeField] private float _maxDistance = 100;
        [SerializeField] private Color _lineColor = Color.gray;

        public int ReflectionTimes
        {
            get => _reflectionTimes;
            set => _reflectionTimes = value;
        }

        private void OnDrawGizmos()
        {
            if (_active) {
                DrawPredictedReflectionPattern(transform.position + transform.forward, transform.forward, _reflectionTimes);
            }
        }

        private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
        {
            while (reflectionsRemaining-- > 0) {
                Vector3 startingPosition = position;

                Ray ray = new Ray(position, direction);
                if (Physics.Raycast(ray, out var hit, _maxDistance)) {
                    direction = Vector3.Reflect(direction, hit.normal);
                    position = hit.point;
                } else {
                    position += direction * _maxDistance;
                }

                Gizmos.color = _lineColor;
                Gizmos.DrawLine(startingPosition, position);
            }
        }
    }
}