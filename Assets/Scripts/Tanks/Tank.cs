using UnityEngine;

namespace Assets.Scripts.Tanks {
    public abstract class Tank : MonoBehaviour {
        private TankHealth _tankHealth;
        public TankHealth Health => _tankHealth;

        protected virtual void Awake()
        {
            _tankHealth = GetComponent<TankHealth>();
        }

        protected virtual void OnEnable()
        {
            if (_tankHealth != null) {
                _tankHealth.OnKill += Kill;
            }
        }

        protected virtual void OnDisable()
        {
            if (_tankHealth != null) {
                _tankHealth.OnKill -= Kill;
            }
        }

        public virtual void Kill()
        {
            if (_tankHealth != null && Health.Invincible) return;
            gameObject.SetActive(false);
        }
    }
}