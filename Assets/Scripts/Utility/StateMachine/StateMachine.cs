using UnityEngine;

namespace Assets.Scripts.Utility.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private bool _debugState = false;

        public IState CurrentState { get; private set; }
        private IState _previousState;
        internal IState DefaultState = new NullState();

        private bool _inTransition;

        public void ChangeState(IState newState)
        {
            if (CurrentState == newState || _inTransition || newState == null) return;

            ChangeStateRoutine(newState);
        }

        public void RevertToPreviousState(bool exitCurrentState = true)
        {
            _previousState ??= DefaultState;
            // Exit current State
            if (exitCurrentState) {
                CurrentState.Exit();
            }
            // Set to the previous state
            if (_debugState) Debug.Log("Reverting to Previous State: " + _previousState);
            CurrentState = _previousState;
            _previousState = null;
            // Enter the new state
            CurrentState.Enter();
        }

        private void ChangeStateRoutine(IState newState)
        {
            _inTransition = true;

            if (CurrentState != null) {
                CurrentState.Exit();
                _previousState = CurrentState;
            }

            if (_debugState) Debug.Log("Changing to New State: " + newState);
            CurrentState = newState;

            CurrentState.Enter();

            _inTransition = false;
        }

        public void Update()
        {
            if (CurrentState == null || _inTransition) return;

            CurrentState.Tick();
        }

        public void FixedUpdate()
        {
            if (CurrentState == null || _inTransition) return;

            CurrentState.FixedTick();
        }
    }
}