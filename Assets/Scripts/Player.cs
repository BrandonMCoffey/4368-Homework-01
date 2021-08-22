using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(TankController))]
    [RequireComponent(typeof(PlayerHealth))]
    public class Player : MonoBehaviour {
        private TankController _tankController;
        private PlayerHealth _health;

        #region Script References

        private void Awake()
        {
            _tankController = GetComponent<TankController>();
            _health = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            _health.OnKill += Kill;
        }

        private void OnDisable()
        {
            _health.OnKill -= Kill;
        }

        public PlayerHealth GetHealth()
        {
            return _health;
        }

        #endregion

        public void Kill()
        {
            gameObject.SetActive(false);
        }

        public void AdjustSpeed(float multiplier, float time)
        {
            if (time < 0) {
                // Permanent Speed Increase
                _tankController.IncreaseSpeed(multiplier);
            } else {
                // Timed Speed Increase
                StartCoroutine(AdjustSpeedTimer(multiplier, time));
            }
        }

        private IEnumerator AdjustSpeedTimer(float multiplier, float time)
        {
            _tankController.IncreaseSpeed(multiplier);
            yield return new WaitForSecondsRealtime(time);
            _tankController.DecreaseSpeed(multiplier);
        }
    }
}