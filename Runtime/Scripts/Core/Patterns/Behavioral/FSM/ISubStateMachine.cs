namespace OSK
{
    public interface ISubStateMachine : IState
    {
        public string GetCurrentStateName();
    }
}