using Assets.Scripts.Utility.CustomFloats;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utility {
    [RequireComponent(typeof(Text))]
    public class VariableText : MonoBehaviour {
        [TextArea, SerializeField] private string _textBefore = "";
        [SerializeField] private FloatVariable _floatReference = null;
        [TextArea, SerializeField] private string _textAfter = "";

        private Text _textField;

        private void Awake()
        {
            _textField = GetComponent<Text>();
        }

        private void Update()
        {
            if (_floatReference == null) return;
            string value = _floatReference.Value.ToString("0");
            _textField.text = _textBefore + value + _textAfter;
        }
    }
}