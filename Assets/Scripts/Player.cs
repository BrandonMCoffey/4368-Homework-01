using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Inventory))]
    public class Player : MonoBehaviour {
        private TankController _tankController;
        private Health _health;
        private Inventory _inventory;

        #region Script References

        private void Awake()
        {
            _tankController = GetComponent<TankController>();
            _health = GetComponent<Health>();
            _inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            _health.OnKill += Kill;
        }

        private void OnDisable()
        {
            _health.OnKill -= Kill;
        }

        public TankController GetTankController()
        {
            return _tankController;
        }

        public Health GetHealth()
        {
            return _health;
        }

        public Inventory GetInventory()
        {
            return _inventory;
        }

        #endregion

        public void Kill()
        {
            gameObject.SetActive(false);
        }
    }
}