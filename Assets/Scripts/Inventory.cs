using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class Inventory : MonoBehaviour {
        [SerializeField] private Text _displayText = null;
        [SerializeField] private string _textToDisplay = "Treasure: ";

        private int _treasureCount;
        public int Treasure => _treasureCount;


        private void OnEnable()
        {
            _treasureCount = 0;
            OnValueChanged();
        }

        public void AddTreasure(int amount = 1)
        {
            _treasureCount += amount;
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            if (_displayText == null) return;
            _displayText.text = _textToDisplay + _treasureCount;
        }
    }
}