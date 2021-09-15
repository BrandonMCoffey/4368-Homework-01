using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Enemies.Basic
{
    public class Killer : Enemy
    {
        protected override bool OnImpact(GameObject other)
        {
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            if (damageableObject == null) {
                return false;
            }
            // Kill the other object
            damageableObject.OnKill();

            return true;
        }
    }
}