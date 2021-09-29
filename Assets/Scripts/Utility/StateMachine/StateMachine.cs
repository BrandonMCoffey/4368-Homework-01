using UnityEngine;

namespace Utility.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private bool _debugState = false;

        public IState CurrentState { get; private set; }
        public IState PreviousState { get; private set; }
        internal IState DefaultState = new NullState();

        protected bool InTransition { get; set; }

        public void ChangeState(IState newState, bool canBeSame = false)
        {
            if (InTransition || newState == null) return;
            if (CurrentState == newState && !canBeSame) return;

            ChangeStateRoutine(newState);
        }

        public void RevertToPreviousState(bool exitCurrentState = true, bool enterNextState = true)
        {
            PreviousState ??= DefaultState;
            // Exit current State
            if (exitCurrentState) {
                CurrentState.Exit();
            }
            // Set to the previous state
            if (_debugState) Debug.Log("Reverting to Previous State: " + PreviousState);
            CurrentState = PreviousState;
            PreviousState = null;
            // Enter the new state
            if (enterNextState) {
                CurrentState.Enter();
            }
            OnStateChanged();
        }

        protected virtual void OnStateChanged()
        {
        }

        private void ChangeStateRoutine(IState newState)
        {
            InTransition = true;

            if (CurrentState != null) {
                CurrentState.Exit();
                if (CurrentState != PreviousState) {
                    PreviousState = CurrentState;
                }
            }

            //if (_debugState) Debug.Log("Changing to New State: " + newState);
            CurrentState = newState;

            CurrentState.Enter();
            OnStateChanged();

            InTransition = false;
        }

        public void Update()
        {
            if (CurrentState == null || InTransition) return;

            CurrentState.Tick();
        }

        public void FixedUpdate()
        {
            if (CurrentState == null || InTransition) return;

            CurrentState.FixedTick();
        }
    }
}