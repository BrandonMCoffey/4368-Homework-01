using System;
using UnityEngine;

namespace Utility.CustomFloats
{
    [CreateAssetMenu]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField] private float _value;

        public float Value
        {
            get => _value;
            set
            {
                OnValueChanged?.Invoke();
                _value = value;
            }
        }

        public event Action OnValueChanged = delegate { };

        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}