using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Tanks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Enemies {
    [RequireComponent(typeof(TankHealth))]
    public class EnemyTank : MonoBehaviour, IDamageable {
        [SerializeField] private EnemyTankData _enemyData = null;
        [SerializeField] private List<MeshRenderer> _baseMaterialsToChange = new List<MeshRenderer>();

        public TankHealth Health { get; private set; }
        public TankMovement Movement { get; private set; }

        private void Awake()
        {
            Health = GetComponent<TankHealth>();
            Movement = GetComponent<TankMovement>();

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

        public void OnTankImpact(int damageTaken)
        {
            throw new System.NotImplementedException();
        }

        public void OnBulletImpact(int damageTaken)
        {
            throw new System.NotImplementedException();
        }

        public void OnKill()
        {
            throw new System.NotImplementedException();
        }
    }
}