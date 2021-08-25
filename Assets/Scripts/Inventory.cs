using Assets.Scripts.Utility.FloatRef;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class Inventory : MonoBehaviour {
        [SerializeField] private Text _displayText = null;
        [SerializeField] private string _textToDisplay = "Treasure: ";
        [SerializeField] private FloatReference _treasureCount = new FloatReference(0);

        public int Treasure => Mathf.FloorToInt(_treasureCount.Value);


        private void OnEnable()
        {
            _treasureCount.Value = 0;
            OnValueChanged();
        }

        public void AddTreasure(int amount = 1)
        {
            _treasureCount.Value += amount;
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            if (_displayText == null) return;
            _displayText.text = _textToDisplay + _treasureCount;
        }
    }
}