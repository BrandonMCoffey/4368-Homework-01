using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Tanks;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(TankHealth))]
    public class EnemyTank : MonoBehaviour, ILockable
    {
        [Header("Tank Settings")]
        [SerializeField] private Material _tankMaterial = null;
        [Header("References")]
        [SerializeField] private List<GameObject> _lockObjects = new List<GameObject>();
        [SerializeField] private List<MeshRenderer> _baseMaterialsToChange = new List<MeshRenderer>();

        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }

        private Collider _collider;

        private void Awake()
        {
            Health = GetComponent<TankHealth>();
            Movement = GetComponent<TankMovement>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (_tankMaterial == null) return;
            foreach (var meshRenderer in _baseMaterialsToChange) {
                meshRenderer.material = _tankMaterial;
            }
        }

        public void Lock(bool active = true)
        {
            foreach (var obj in _lockObjects) {
                obj.SetActive(!active);
            }
            _collider.enabled = !active;
        }
    }
}