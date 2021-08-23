using UnityEngine;

namespace Assets.Scripts.Powerups {
    [RequireComponent(typeof(Collider))]
    public class PowerupBase : MonoBehaviour {
        [SerializeField] private float _powerupDuration = 5;

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) {
            }
        }
    }
}