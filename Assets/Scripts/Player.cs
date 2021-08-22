using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    public class Player : MonoBehaviour {
        private TankController _tankController;

        private void Awake()
        {
            _tankController = GetComponent<TankController>();
        }

        public void Kill()
        {
            gameObject.SetActive(false);
        }
    }
}