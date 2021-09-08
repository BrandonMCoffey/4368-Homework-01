using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(TankHealth))]
    public class EnemyTank : MonoBehaviour, ILockable
    {
        [SerializeField] private EnemyTankData _enemyData = null;
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

            if (_enemyData == null) DebugHelper.Error(gameObject, "No attached enemy data");
        }

        private void Start()
        {
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (_enemyData == null || _enemyData.Material == null) return;
            foreach (var meshRenderer in _baseMaterialsToChange) {
                meshRenderer.material = _enemyData.Material;
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