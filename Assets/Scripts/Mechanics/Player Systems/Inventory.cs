using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;

namespace Assets.Scripts.Mechanics.Player_Systems
{
    public class Inventory : MonoBehaviour
    {
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