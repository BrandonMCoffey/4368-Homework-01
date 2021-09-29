using UnityEngine;
using UnityEngine.UI;
using Utility.CustomFloats;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class VariableSlider : MonoBehaviour
    {
        [SerializeField] private FloatVariable _variable = null;

        private Slider _slider;
        private float _max;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
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
            Debug.Log("Test");
            float value = _variable.Value;
            if (value > _max) {
                _max = value;
            }
            _slider.value = value / _max;
        }
    }
}