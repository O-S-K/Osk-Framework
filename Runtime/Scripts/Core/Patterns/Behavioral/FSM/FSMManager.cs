using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace OSK
{
    public class FSMManager : GameFrameworkComponent
    {
        private Dictionary<string, GroupState> stateGroups = new Dictionary<string, GroupState>();
        public GroupState CurrentGroup => _current;
        public GroupState PreviousGroup => _previous;

        [CustomInspector.ReadOnly, CustomInspector.ShowInInspector]
        private GroupState _current;

        [CustomInspector.ReadOnly, CustomInspector.ShowInInspector]
        private GroupState _previous;


        public IStateMachine CurrentState => _current?.CurrentState;
        public IStateMachine PreviousState => _previous?.PreviousState;

        [CustomInspector.ReadOnly, CustomInspector.ShowInInspector]
        private bool _pause = false;
        
        
        public override void OnInit() {}


        public void Pause(bool isPause)
        {
            _pause = isPause;
        }

        public void Switch(string groupName, IStateMachine newState)
        {
            if (!stateGroups.TryGetValue(groupName, out var group))
            {
                OSK.Logg.LogError($"State group '{groupName}' not found.");
                return;
            }

            stateGroups[groupName].Switch(newState, new[] { true });
        }

        public void Switch(string groupName, IStateMachine newState, bool condition)
        {
            if (!stateGroups.TryGetValue(groupName, out var group))
            {
                OSK.Logg.LogError($"State group '{groupName}' not found.");
                return;
            }

            stateGroups[groupName].Switch(newState, new[] { condition });
        }


        public void Switch(string groupName, IStateMachine newState, bool[] condition)
        {
            if (!stateGroups.TryGetValue(groupName, out var group))
            {
                OSK.Logg.LogError($"State group '{groupName}' not found.");
                return;
            }

            stateGroups[groupName].Switch(newState, condition);
        }


        public void Finalize(string groupName)
        {
            if (stateGroups.TryGetValue(groupName, out var group))
            {
                group.CurrentState.Exit();
                group.CurrentState = null;
                _current = null;
                _previous = null;
            }
            else
            {
                OSK.Logg.LogError($"State group '{groupName}' not found.");
            }
        }


        public void Exit(string groupName)
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
            if (_pause)
                return;

            foreach (var group in stateGroups.Values)
            {
                if (group.isTransition)
                    group.CurrentState?.Tick();
            }
        }

        public virtual void FixedUpdate()
        {
            if (_pause)
                return;
            foreach (var group in stateGroups.Values)
            {
                if (group.isTransition)
                    group.CurrentState?.FixedTick();
            }
        }

        public GroupState CreateGroup(string groupName)
        {
            if (!stateGroups.ContainsKey(groupName))
            {
                stateGroups[groupName] = new GroupState(groupName)
                {
                    States = new List<IStateMachine>()
                };
                _current = stateGroups[groupName];
                _previous = null;
                OSK.Logg.Log($"Created state group '{groupName}'.");
            }
            else
            {
                OSK.Logg.LogWarning($"State group '{groupName}' already exists.");
            }

            return stateGroups[groupName];
        }

        public void AddStateToGroup(string groupName, IStateMachine state)
        {
            if (stateGroups.ContainsKey(groupName))
            {
                stateGroups[groupName].Add(state);
                OSK.Logg.Log($"Added state to group '{groupName}': {state}");
            }
            else
            {
                OSK.Logg.LogError($"State group '{groupName}' not found.");
            }
        }

        public void SetCurrentGroup(GroupState group)
        {
            if (_current != null)
            {
                _previous = _current;
            }

            _current = group;
            OSK.Logg.Log($"Current group: {group.Name}");
        }

        public List<GroupState> GetGroups()
        {
            return new List<GroupState>(stateGroups.Values);
        }

        public GroupState GetGroup(string groupName)
        {
            return stateGroups.GetValueOrDefault(groupName);
        }

        public void RemoveStateFromGroup(string groupName, IStateMachine state)
        {
            if (stateGroups.ContainsKey(groupName))
            {
                stateGroups[groupName].RemoveState(state);
                OSK.Logg.Log($"Removed state from group '{groupName}': {state}");
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
                stateGroups[groupName].Clear();
                stateGroups.Remove(groupName);
                OSK.Logg.Log($"Removed group '{groupName}'.");
            }
            else
            {
                OSK.Logg.LogError($"Group '{groupName}' not found.");
            }
        }
    }
}