using System.Collections.Generic;
using OSK;

public class StateGroup
{
    public string Name { get; }
    public List<IStateMachine> States { get; }
    public IStateMachine CurrentState { get; set; }
    public IStateMachine PreviousState { get; set; }

    public StateGroup(string name)
    {
        Name = name;
        States = new List<IStateMachine>();
    }

    public void SetCurrentState(IStateMachine newState)
    {
        CurrentState.Exit();
        PreviousState?.Exit();
        
        CurrentState = newState;
        CurrentState.Enter(); 
        Logg.Log($"StateGroup: {Name.Color(LogColor.Yellow)} CurrentState: {CurrentState.StateName.Color(LogColor.Orange)}");
    }

    public void ExitState()
    {
        PreviousState = CurrentState;
        CurrentState = null;
    }
}