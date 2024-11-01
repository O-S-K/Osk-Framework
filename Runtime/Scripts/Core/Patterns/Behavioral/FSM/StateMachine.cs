using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace OSK
{
    public class StateMachine
    {
        private readonly Dictionary<IState, List<Transition>> _transitions = new Dictionary<IState, List<Transition>>();
        private readonly List<Transition> _anyTransitions = new List<Transition>();
        private static readonly List<Transition> EmptyTransitions = new List<Transition>(0);
        private List<Transition> _currentTransitions = new List<Transition>();
        private IState _currentState;

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                // check transition if condition is met
                Set(transition.To);
            }

            _currentState?.Tick();
        }

        public void FixedTick()
        {
            _currentState?.FixedTick();
        }

        // Start 
        public void Init(IState state) => Set(state);

        private void Set(IState state)
        {
            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState, out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;

            _currentState.OnEnter();
        }

        public void Add(IState[] states)
        {
            foreach (var state in states)
            {
                Add(state);
            }
        }

        public void Add(IState state)
        {
            if (_transitions.ContainsKey(state))
                return;
            _transitions.Add(state, new List<Transition>());
        }

        public void At(IState from, IState to, Func<bool> predicate)
        {
            // Check if there are already transitions from the 'from' state
            if (!_transitions.TryGetValue(from, out var transitions))
            {
                // No transitions from the 'from' state, create a new list
                transitions = new List<Transition>();
                // Add to dict
                _transitions[from] = transitions;
            }

            // Add the transition we passed in to the List of transitions of the 'from' state
            transitions.Add(new Transition(from, to, predicate));
        }

        public void Any(IState state, Func<bool> predicate)
        {
            // Add the transition to the list of any transitions
            _anyTransitions.Add(new Transition(null, state, predicate));
        }

        public void Remove(IState state)
        {
            if (_transitions.ContainsKey(state))
            {
                _transitions.Remove(state);
            }
        }

        public void RemoveAll()
        {
            _transitions.Clear();
            _anyTransitions.Clear();
            _currentTransitions.Clear();
            _currentState = null;
        }


        public List<IState> GetStates()
        {
            return _transitions.Keys.ToList();
        }

        private Transition GetTransition()
        {
            // Iterate through all any state transitions first
            foreach (var transition in _anyTransitions.Where(transition => transition.Condition()
                                                                           // Dont Allow Any to Same State Transition.
                                                                           && transition.To != _currentState))
            {
                // If a condition is met, return that transition
                return transition;
            }

            // If no 'any' transition is found, check the current state's transitions
            return _currentTransitions.FirstOrDefault(transition => transition.Condition());
        }


        public void Exit()
        {
            _currentState?.OnExit();
            _currentState = null;
        }

        private class Transition
        {
            public IState From { get; }
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState from, IState to, Func<bool> condition)
            {
                From = from;
                To = to;
                Condition = condition;
            }
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }

        public Color GetGizmoColor()
        {
            return _currentState?.GizmoState() ?? Color.black;
        }

        private string GetCurrentStateName()
        {
            return _currentState?.GetType().Name ?? "No State";
        }
    }
}