using System.Collections.Generic;
using OSK;
using UnityEngine;

namespace OSK
{
    // Example:
    //stateGroup = Main.State.CreateGroup("BotStateExample");
    //stateGroup.Add(new BotIdleStateExample());
    //stateGroup.Add(new BotDieStateExample());
    //stateGroup.Init(new BotIdleStateExample());
    
    [System.Serializable]
    public class GroupState
    {
        public string Name { get; }
        [SerializeReference] public List<IStateMachine> States;
        [SerializeReference] public IStateMachine CurrentState;
        [SerializeReference] public IStateMachine PreviousState;
        public bool isTransition = true;

        public GroupState(string name)
        {
            Name = name;
            States = new List<IStateMachine>();
        }
        
        public void Init(IStateMachine newState)
        {
            CurrentState = newState;
            CurrentState.Enter();
            OSK.Logg.Log(newState.ToString());
        }
         
        public void Add(IStateMachine state)
        {
            if (!States.Contains(state))
            {
                States.Add(state);
            }
            else
            {
                Logg.LogError($"State: {state.StateName.Color(ColorCustom.Cyan)} already exists in StateGroup: {Name.Color(ColorCustom.Green)}");
            }
        }
        
        private bool IsCheckCondition(bool[] condition)
        {
            foreach (var item in condition)
            {
                if (!item)
                {
                    //Logg.Log("Condition is not met");
                    return false;
                }
            }

            return true;
        }
        
        public void Switch(IStateMachine newState, bool[] condition)
        { 
            if (condition != null && !IsCheckCondition(condition))
            {
                Logg.Log("Condition is not met");
                return;
            }
            Switch(newState);
        }
        
        public void Switch(IStateMachine newState)
        { 
            if (CurrentState == newState)
            {
                Logg.Log("Current state is the same as new state");
                return;
            } 

            isTransition = true;
            if (CurrentState != null)
            {
                PreviousState = CurrentState;
                CurrentState.Exit();
            }

            Main.Fsm.SetCurrentGroup(this);
            CurrentState = newState;
            CurrentState?.Enter();
            isTransition = false;
        }

        public void ExitState()
        {
            if (CurrentState != null)
            {
                PreviousState = CurrentState;
                CurrentState = null;
            }
        }
        
        public void RemoveState(IStateMachine state)
        {
            if (States.Contains(state))
            {
                States.Remove(state);
            }
            else
            {
                Logg.LogError($"State: {state.StateName.Color(ColorCustom.Cyan)} not found in StateGroup: {Name.Color(ColorCustom.Green)}");
            }
        }
        
        public void Clear()
        {
            States.Clear();
            CurrentState = null;
            PreviousState = null;
        }
    }
}