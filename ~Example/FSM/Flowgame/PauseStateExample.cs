using OSK;
using UnityEngine;


public class PauseStateExample : IState
{
    public string StateName => "PauseStateExample";
    FlowgameExample flowgameExample;
    public PauseStateExample(FlowgameExample flowgameExample)
    {
        this.flowgameExample = flowgameExample;
    }
 
    public void OnEnter()
    {
        Debug.Log("PauseStateExample");
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