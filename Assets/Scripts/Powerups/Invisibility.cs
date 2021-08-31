using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Powerups {
    public class Invisibility : PowerupBase {
        private IInvisibile _effected;

        protected override bool OnCollect(GameObject other)
        {
            IInvisibile invisibileObject = other.GetComponent<IInvisibile>();
            if (invisibileObject == null) {
                return false;
            }
            _effected = invisibileObject;

            return true;
        }


        protected override void Activate()
        {
            _effected.OnSetInvisible();
        }

        protected override void Deactivate()
        {
            _effected.OnRemoveInvisible();
        }
    }
}