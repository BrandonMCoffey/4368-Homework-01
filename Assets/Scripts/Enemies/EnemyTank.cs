using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.Enemies {
    public abstract class EnemyTank : MonoBehaviour {
        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }

        private void Awake()
        {
            Health = GetComponent<TankHealth>();
            Movement = GetComponent<TankMovement>();
        }
    }
}