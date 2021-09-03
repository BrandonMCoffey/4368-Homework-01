using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Tanks
{
    public class TankPowerup : MonoBehaviour, IInvincible, IInvisibile
    {
        [Header("Invincibility Settings")]
        [SerializeField] private Material _invincibilityMaterial = null;
        [SerializeField] private List<MeshRenderer> _materialsToChangeWhenInvincible = new List<MeshRenderer>();
        private List<Material> _regularMaterial;

        [Header("Invisibility Settings")]
        [SerializeField] private List<GameObject> _regularArt = new List<GameObject>();
        [SerializeField] private List<GameObject> _invisibleArt = new List<GameObject>();

        private TankHealth _health;

        private void Awake()
        {
            _health = GetComponent<TankHealth>();
        }

        private void Start()
        {
            OnSetInvisible();
        }

        public void OnSetInvincible()
        {
            if (_health == null) return;
            _health.Invincible = true;
            _regularMaterial = new List<Material>(_materialsToChangeWhenInvincible.Count);
            foreach (var meshRenderer in _materialsToChangeWhenInvincible)
            {
                _regularMaterial.Add(meshRenderer.material);
                meshRenderer.material = _invincibilityMaterial;
            }
        }

        public void OnRemoveInvincible()
        {
            if (_health == null) return;
            _health.Invincible = false;
            for (int i = 0; i < _materialsToChangeWhenInvincible.Count; ++i)
            {
                _materialsToChangeWhenInvincible[i].material = _regularMaterial[i];
            }
            _regularMaterial.Clear();
        }

        public void OnRemoveInvisible()
        {
            foreach (var obj in _invisibleArt)
            {
                obj.SetActive(false);
            }
            foreach (var obj in _regularArt)
            {
                obj.SetActive(true);
            }
        }

        // TODO: Does disabling object break Invincible MeshRenderers?
        public void OnSetInvisible()
        {
            foreach (var obj in _invisibleArt)
            {
                obj.SetActive(true);
            }
            foreach (var obj in _regularArt)
            {
                obj.SetActive(false);
            }
        }
    }
}
