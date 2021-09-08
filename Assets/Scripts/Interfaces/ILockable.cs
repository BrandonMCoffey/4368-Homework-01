using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ILockable
    {
        void Lock(bool active = true);
    }
}