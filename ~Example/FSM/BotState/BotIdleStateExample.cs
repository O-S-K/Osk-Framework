using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class BotIdleStateExample : IState
{
    public string StateName => "BotIdleStateExample";
    private BotStateExample botStateExample;

    public BotIdleStateExample(BotStateExample botStateExample)
    {
        this.botStateExample = botStateExample;
    }

    public void OnEnter()
    {
        Debug.Log("Idle");
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