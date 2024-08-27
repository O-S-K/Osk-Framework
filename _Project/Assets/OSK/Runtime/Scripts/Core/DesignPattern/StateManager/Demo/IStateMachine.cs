public interface IStateMachine
{
    void Enter();
    void Tick();
    void FixedTick();
    void Exit();
}
