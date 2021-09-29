using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility.GameEvents.Logic;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class DamageVignette : MonoBehaviour
    {
        [SerializeField] private float _duration = 1;
        [SerializeField] private float _maxAlpha = 1;

        private Image _image;
        private Coroutine _coroutine;

        private void Awake()
        {
            _image = GetComponent<Image>();
            SetAlpha(0);
        }

        public void Damage()
        {
            if (_coroutine != null) {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(FlashDamage());
        }

        private IEnumerator FlashDamage()
        {
            float start = _image.color.a / _maxAlpha;
            for (float t = start / 2; t < _duration; t += Time.deltaTime) {
                float delta = 1 - Mathf.Abs(1 - 2 * t / _duration);
                SetAlpha(delta);
                yield return null;
            }
            _coroutine = null;
        }

        private void SetAlpha(float alpha)
        {
            Color col = _image.color;
            col.a = _maxAlpha * alpha;
            _image.color = col;
        }
    }
}