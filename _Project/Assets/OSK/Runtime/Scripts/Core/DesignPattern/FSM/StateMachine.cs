using System.Collections.Generic;
using UnityEngine;

public class StateMachine : GameFrameworkComponent
{
    private Dictionary<string, StateGroup> stateGroups = new Dictionary<string, StateGroup>();
    private StateGroup currentGroup;
    private StateGroup previousGroup;

    public IStateMachine CurrentState => currentGroup?.CurrentState;
    public IStateMachine PreviousState => previousGroup?.PreviousState;

    protected bool isTransition = false;
    protected bool pause = false;

    public void SetPause(bool isPause)
    {
        pause = isPause;
    }

    public void InitState(string groupName, IStateMachine newState)
    {
        if (stateGroups.TryGetValue(groupName, out var group))
        {
            group.CurrentState = newState;
            currentGroup = group;
            previousGroup = null;
            currentGroup.CurrentState.Enter();
            OSK.Logg.Log(newState.ToString());
        }
        else
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
        }
    }

    public void SwitchState(string groupName, IStateMachine newState)
    {
        if (!stateGroups.TryGetValue(groupName, out var group))
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
            return;
        }

        if (group.CurrentState == newState || isTransition)
        {
            return;
        }

        UpdateState(group, newState);
        OSK.Logg.Log(newState.ToString());
    }
     
    private void UpdateState(StateGroup group, IStateMachine newState)
    {
        isTransition = true;
        if (group.CurrentState != null)
        {
            group.CurrentState.Exit();
        }

        if (previousGroup != null)
        {
            previousGroup = group;
        }

        group.CurrentState = newState;
        group.CurrentState.Enter();
        currentGroup = group;
        isTransition = false;
    }
    
    public void FinalizeState(string groupName)
    {
        if (stateGroups.TryGetValue(groupName, out var group))
        {
            group.CurrentState.Exit();
            group.CurrentState = null;
            currentGroup = null;
            previousGroup = null;
        }
        else
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
        }
    }
    
    
    public void ExitState(string groupName)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName].ExitState();
        }
        else
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
        }
    }


    public virtual void Update()
    {
        if (!isTransition && !pause)
        {
            foreach (var group in stateGroups.Values)
            {
                group.CurrentState?.Tick();
            }
        }
    }

    public virtual void FixedUpdate()
    {
        if (!isTransition && !pause)
        {
            foreach (var group in stateGroups.Values)
            {
                group.CurrentState?.FixedTick();
            }
        }
    }

    public string CreateGroup(string groupName)
    {
        if (!stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName] = new StateGroup(groupName);
            OSK.Logg.Log($"Created state group '{groupName}'.");
        }
        else
        {
            OSK.Logg.LogWarning($"State group '{groupName}' already exists.");
        }
        return groupName;
    }
    
    public void AddListStateToGroup(string groupName, List<IStateMachine> states)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName].States.AddRange(states);
            OSK.Logg.Log($"Added list state to group '{groupName}': {states.Count}");
        }
        else
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
        }
    }

    public void AddStateToGroup(string groupName, IStateMachine state)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName].States.Add(state);
            OSK.Logg.Log($"Added state to group '{groupName}': {state}");
        }
        else
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
        }
    }

    public List<StateGroup> GetGroups()
    {
        return new List<StateGroup>(stateGroups.Values);
    }

    public StateGroup GetGroup(string groupName)
    {
        return stateGroups.GetValueOrDefault(groupName);
    }
    
    
    public void RemoveStateFromGroup(string groupName)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups.Remove(groupName);
            OSK.Logg.Log($"Removed state group '{groupName}'.");
        }
        else
        {
            OSK.Logg.LogError($"State group '{groupName}' not found.");
        }
    }

    public void RemoveGroup(string groupName)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups.Remove(groupName);
            OSK.Logg.Log($"Removed group '{groupName}'.");
        }
        else
        {
            OSK.Logg.LogError($"Group '{groupName}' not found.");
        }
    }
}