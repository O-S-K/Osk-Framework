using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStateExample : MonoBehaviour
{
    private void Start()
    {
        var group= Main.State.CreateGroup("BotStateExample");
        
        StateGroup stateGroup = new StateGroup(group);
        stateGroup.States.Add(new BotIdleStateExample());
        stateGroup.States.Add(new BotAttackStateExample());
        stateGroup.States.Add(new BotDieStateExample());
        
        Main.State.AddListStateToGroup(group, stateGroup.States);
        Main.State.InitState(group, new BotIdleStateExample());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Main.State.SwitchState("BotStateExample", new BotAttackStateExample());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Main.State.SwitchState("BotStateExample", new BotDieStateExample());
        }
    }
}
