using System.Collections.Generic;
using OSK;
using UnityEngine;

namespace OSK
{
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
        Logg.Log($"StateGroup: {Name.Color(ColorCustom.Green)} CurrentState: {CurrentState.StateName.Color(ColorCustom.Cyan)}");
    }

    public void ExitState()
    {
        PreviousState = CurrentState;
        CurrentState = null;
    }
}
}