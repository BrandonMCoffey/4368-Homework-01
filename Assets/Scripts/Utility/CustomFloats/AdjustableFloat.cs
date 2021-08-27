using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility.CustomFloats {
    public enum ValueAdjustType {
        AddRaw,
        AddBase,
        Multiply,
    }

    public class AdjustableFloat {
        private float _baseValue;
        public float Value { get; private set; }
        public int ActiveEffects { get; private set; }

        public void SetBaseValue(float baseValue)
        {
            _baseValue = baseValue;
            Value = baseValue;
        }

        public IEnumerator AdjustValueOverTime(ValueAdjustType type, float amount, float timer)
        {
            ActiveEffects++;
            IncreaseValue(type, amount);
            yield return new WaitForSecondsRealtime(timer);
            ActiveEffects--;
            DecreaseValue(type, amount);
        }

        public void IncreaseValue(ValueAdjustType type, float amount)
        {
            Value = type switch
            {
                ValueAdjustType.AddRaw   => Value + amount,
                ValueAdjustType.AddBase  => Value + _baseValue * amount,
                ValueAdjustType.Multiply => Value * amount,
                _                        => Value
            };
        }

        public void DecreaseValue(ValueAdjustType type, float amount)
        {
            Value = type switch
            {
                ValueAdjustType.AddRaw   => Value - amount,
                ValueAdjustType.AddBase  => Value - _baseValue * amount,
                ValueAdjustType.Multiply => Value / amount,
                _                        => Value
            };
        }
    }
}