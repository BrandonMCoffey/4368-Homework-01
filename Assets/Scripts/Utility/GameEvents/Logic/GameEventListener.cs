using UnityEngine;

namespace Assets.Scripts.Utility.GameEvents.Logic {
    public class GameEventListener : MonoBehaviour {
        [SerializeField] private GameEvent _event = null;

        private void Awake()
        {
            if (_event == null) DebugHelper.Warn("No Attached Event");
        }

        private void OnEnable()
        {
            if (_event != null) _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (_event != null) _event.UnRegisterListener(this);
        }

        public virtual void OnEventRaised()
        {
        }
    }
}