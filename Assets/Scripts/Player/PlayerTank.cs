using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.Player {
    [RequireComponent(typeof(TankHealth))]
    [RequireComponent(typeof(Inventory))]
    public class PlayerTank : MonoBehaviour {
        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }
        public Inventory Inventory { get; private set; }

        private void Awake()
        {
            Health = GetComponent<TankHealth>();
            Movement = GetComponent<TankMovement>();
            Inventory = GetComponent<Inventory>();
        }
    }
}