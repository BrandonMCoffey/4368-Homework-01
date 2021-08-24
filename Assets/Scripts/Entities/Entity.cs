using UnityEngine;

namespace Assets.Scripts.Entities {
    public abstract class Entity : MonoBehaviour {
        private EntityHealth _entityHealth;
        public EntityHealth Health => _entityHealth;

        protected virtual void Awake()
        {
            _entityHealth = GetComponent<EntityHealth>();
        }

        protected virtual void OnEnable()
        {
            if (_entityHealth != null) {
                _entityHealth.OnKill += Kill;
            }
        }

        protected virtual void OnDisable()
        {
            if (_entityHealth != null) {
                _entityHealth.OnKill -= Kill;
            }
        }

        public virtual void Kill()
        {
            if (_entityHealth != null && Health.Invincible) return;
            gameObject.SetActive(false);
        }
    }
}