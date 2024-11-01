using OSK;
using UnityEngine;

public class IngameStateExample : IState
{
    public string StateName => "IngameStateExample";
    public FlowgameExample flowgameExample;
    public IngameStateExample(FlowgameExample flowgameExample)
    {
        this.flowgameExample = flowgameExample;
    }

    public void OnEnter()
    {
        Debug .Log("IngameStateExample");
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