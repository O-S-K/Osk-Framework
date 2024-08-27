using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public IStateMachine CurrentState { get; set; }
    public IStateMachine PreviousState { get; set; }

    protected bool isTransition = false;
    protected bool pause = false;

    public void SetPause(bool isPause)
    {
        pause = isPause;
    }

    public void ExitState()
    {
        Debug.Log("Exit Current State "
            + CurrentState.ToString());
        CurrentState = null;
        PreviousState = null;
        isTransition = false;
    }

    public virtual void ChangeState(IStateMachine newState)
    {
        if(CurrentState == newState || isTransition)
        {
            return;
        }
        UpdateState(newState);
        Debug.Log(newState.ToString());
    }

    protected virtual void UpdateState(IStateMachine newState)
    {
        isTransition = true;
        if(CurrentState != null)
        {
            CurrentState.Exit();
        }

        if(PreviousState != null)
        {
            PreviousState = CurrentState;
        }

        CurrentState = newState;
        CurrentState.Enter();

        isTransition = false;
    }

    public virtual void Update()
    {
        if(CurrentState != null && !isTransition && !pause)
        {
            CurrentState.Tick();
        }
    }

    public virtual void FixedUpdate()
    {
        if (CurrentState != null && !isTransition && !pause)
        {
            CurrentState.FixedTick();
        }
    }

}
