using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Level_Systems
{
    public class DamageVolume : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _delay = 0.1f;

        private List<IDamageable> _delayDamageObjects = new List<IDamageable>();
        private float _timer;


        private void OnEnable()
        {
            _delayDamageObjects = new List<IDamageable>();
            _timer = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _delay) {
                _delayDamageObjects = _delayDamageObjects.Where(item => item != null).ToList();
                foreach (var damageable in _delayDamageObjects) {
                    bool killed = damageable.OnDamageVolume(_damage);
                    if (killed) {
                        _delayDamageObjects.Remove(damageable);
                    }
                }
                _timer = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (_delayDamageObjects.Contains(damageable)) return;
            _delayDamageObjects.Add(damageable);
        }

        private void OnTriggerExit(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (_delayDamageObjects.Contains(damageable)) {
                _delayDamageObjects.Remove(damageable);
            }
        }
    }
}