using Assets.Scripts.Utility.CustomFloats;

namespace Assets.Scripts.Interfaces
{
    public interface IMoveable
    {
        void OnSpeedIncrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw);
        void OnSpeedDecrease(float amount, float duration, ValueAdjustType type = ValueAdjustType.AddRaw);
    }
}