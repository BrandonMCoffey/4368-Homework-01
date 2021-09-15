using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Level_Systems
{
    [CreateAssetMenu]
    public class SpawnableObjectData : ScriptableObject
    {
        [SerializeField] private bool _weighted = true;
        [SerializeField] private List<DataField> _field = new List<DataField>();

        public GameObject GetRandom()
        {
            return _weighted ? GetRandomWeighted() : GetRandomRaw();
        }

        private GameObject GetRandomRaw()
        {
            if (_field.Count == 0) return null;
            int rand = Random.Range(0, _field.Count);
            return _field[rand].Object;
        }

        private GameObject GetRandomWeighted()
        {
            if (_field.Count == 0) return null;
            float sum = TotalWeight();
            float rand = Random.Range(0, sum);
            foreach (var data in _field) {
                if (rand <= data.Weight) {
                    return data.Object;
                }
                rand -= data.Weight;
            }
            return null;
        }

        private float TotalWeight()
        {
            return _field.Sum(data => data.Weight);
        }
    }

    [System.Serializable]
    public struct DataField
    {
        public GameObject Object;
        public float Weight;
    }
}