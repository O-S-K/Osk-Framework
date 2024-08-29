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

    public void ExitState(string groupName)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName].ExitState();
        }
        else
        {
            Debug.LogError($"State group '{groupName}' not found.");
        }
    }

    public void InitState(string groupName, IStateMachine newState)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            var group = stateGroups[groupName];
            group.CurrentState = newState;
            currentGroup = group;
            previousGroup = null;
            currentGroup.CurrentState.Enter();
            Debug.Log(newState.ToString());
        }
        else
        {
            Debug.LogError($"State group '{groupName}' not found.");
        }
    }

    public void ChangeState(string groupName, IStateMachine newState)
    {
        if (!stateGroups.ContainsKey(groupName))
        {
            Debug.LogError($"State group '{groupName}' not found.");
            return;
        }

        var group = stateGroups[groupName];
        if (group.CurrentState == newState || isTransition)
        {
            return;
        }

        UpdateState(group, newState);
        Debug.Log(newState.ToString());
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
            Debug.Log($"Created state group '{groupName}'.");
        }
        else
        {
            Debug.LogWarning($"State group '{groupName}' already exists.");
        }
        return groupName;
    }
    
    public void AddListStateToGroup(string groupName, List<IStateMachine> states)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName].States.AddRange(states);
            Debug.Log($"Added list state to group '{groupName}': {states.Count}");
        }
        else
        {
            Debug.LogError($"State group '{groupName}' not found.");
        }
    }

    public void AddStateToGroup(string groupName, IStateMachine state)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups[groupName].States.Add(state);
            Debug.Log($"Added state to group '{groupName}': {state}");
        }
        else
        {
            Debug.LogError($"State group '{groupName}' not found.");
        }
    }

    public void RemoveStateFromGroup(string groupName)
    {
        if (stateGroups.ContainsKey(groupName))
        {
            stateGroups.Remove(groupName);
            Debug.Log($"Removed state group '{groupName}'.");
        }
        else
        {
            Debug.LogError($"State group '{groupName}' not found.");
        }
    }

    public List<StateGroup> GetGroups()
    {
        return new List<StateGroup>(stateGroups.Values);
    }

    public StateGroup GetGroup(string groupName)
    {
        return stateGroups.ContainsKey(groupName) ? stateGroups[groupName] : null;
    }
}