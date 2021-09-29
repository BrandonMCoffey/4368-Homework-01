using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility.CustomFloats;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class VariableSlider : MonoBehaviour
    {
        [SerializeField] private FloatVariable _variable = null;
        [SerializeField] private bool _animateOnStart = true;
        [SerializeField] private float _offset = 2;
        [SerializeField] private float _timeToAnimate = 4;

        private Slider _slider;
        private float _max;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            if (_animateOnStart) {
                StartCoroutine(AnimateSlider());
            }
        }

        private void OnEnable()
        {
            if (_variable == null) return;
            _variable.OnValueChanged += UpdateSlider;
        }

        private void OnDisable()
        {
            if (_variable == null) return;
            _variable.OnValueChanged -= UpdateSlider;
        }

        private void UpdateSlider()
        {
            if (_animateOnStart) return;
            float value = _variable.Value;
            if (value > _max) {
                _max = value;
            }
            _slider.value = value / _max;
        }

        public void SkipAnimation()
        {
            StopAllCoroutines();
            UpdateSlider(1);
        }

        private void UpdateSlider(float delta)
        {
            _slider.value = delta;
        }

        private IEnumerator AnimateSlider()
        {
            yield return new WaitForSecondsRealtime(_offset);
            for (float t = 0; t < _timeToAnimate; t += Time.deltaTime) {
                float delta = t / _timeToAnimate;
                UpdateSlider(delta);
                yield return null;
            }
            _animateOnStart = false;
        }
    }
}