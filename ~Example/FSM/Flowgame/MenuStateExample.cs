using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class MenuStateExample : IState
{
    public string StateName => "MenuStateExample";
    public FlowgameExample flowgameExample;
    public MenuStateExample(FlowgameExample flowgameExample)
    {
        this.flowgameExample = flowgameExample;
    }

    public void OnEnter()
    {
        Debug.Log("MenuStateExample");
    }

    public void Tick()
    {
    }

    public void FixedTick()
    {
    }

    public void OnExit()
    {
    }   
}