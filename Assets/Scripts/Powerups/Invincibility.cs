using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Powerups
{
    public class Invincibility : PowerupBase
    {
        private IInvincible _effected;

        protected override bool OnCollect(GameObject other)
        {
            IInvincible invincibleObject = other.GetComponent<IInvincible>();
            if (invincibleObject == null) {
                return false;
            }
            _effected = invincibleObject;

            return true;
        }

        protected override void Activate()
        {
            _effected.OnSetInvincible();
        }

        protected override void Deactivate()
        {
            _effected.OnRemoveInvincible();
        }
    }
}