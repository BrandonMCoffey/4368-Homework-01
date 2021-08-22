using UnityEngine;

namespace Assets.Scripts {
    [RequireComponent(typeof(Player))]
    public class PlayerHealth : MonoBehaviour {
        [SerializeField] private int _maxHealth = 3;

        private int _currentHealth;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void IncreaseHealth(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        }

        public void DecreaseHealth(int amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0) {
                _player.Kill();
            }
        }
    }
}