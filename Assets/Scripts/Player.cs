using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    [RequireComponent(typeof(PlayerHealth))]
    public class Player : MonoBehaviour {
        private TankController _tankController;
        public PlayerHealth Health;

        #region Script References

        private void Awake()
        {
            _tankController = GetComponent<TankController>();
            Health = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            Health.OnKill += Kill;
        }

        private void OnDisable()
        {
            Health.OnKill -= Kill;
        }

        #endregion

        public void Kill()
        {
            gameObject.SetActive(false);
        }
    }
}