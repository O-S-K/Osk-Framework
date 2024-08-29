using System.Collections.Generic;

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

    public void ExitState()
    {
        PreviousState = CurrentState;
        CurrentState = null;
    }
}