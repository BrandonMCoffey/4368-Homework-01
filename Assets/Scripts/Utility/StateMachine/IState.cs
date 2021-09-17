namespace Assets.Scripts.Utility.StateMachine
{
    public interface IState
    {
        void Enter();
        void Tick();
        void FixedTick();
        void Exit();
    }
}