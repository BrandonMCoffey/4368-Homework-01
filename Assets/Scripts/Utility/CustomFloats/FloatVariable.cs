using System;
using UnityEngine;

namespace Utility.CustomFloats
{
    [CreateAssetMenu]
    public class FloatVariable : ScriptableObject
    {
        public float Value;
        public event Action OnValueChanged = delegate { };

        public void SetValue(float value)
        {
            Value = value;
            OnValueChanged?.Invoke();
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
            OnValueChanged?.Invoke();
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
            OnValueChanged?.Invoke();
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
            OnValueChanged?.Invoke();
        }
    }
}