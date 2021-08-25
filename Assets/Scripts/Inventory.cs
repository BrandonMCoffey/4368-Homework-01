using Assets.Scripts.Utility.FloatRef;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class Inventory : MonoBehaviour {
        [SerializeField] private FloatReference _treasureCount = new FloatReference(0);

        private void OnEnable()
        {
            _treasureCount.Value = 0;
        }

        public void AddTreasure(int amount = 1)
        {
            _treasureCount.Value += amount;
        }
    }
}