public interface IStateMachine
{
    string StateName { get; }
    void Enter();
    void Tick();
    void FixedTick();
    void Exit();
    
}
