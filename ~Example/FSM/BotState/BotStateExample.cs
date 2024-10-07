using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSK;

public class BotStateExample : MonoBehaviour
{
    private string keyGroupStateBot = "BotStateExample";
    
    private void Start()
    {
        var group= Main.State.CreateGroup(keyGroupStateBot);
        StateGroup stateGroup = new StateGroup(group);
        stateGroup.States.Add(new BotIdleStateExample());
        stateGroup.States.Add(new BotAttackStateExample());
        stateGroup.States.Add(new BotDieStateExample());
        
        Main.State.AddListToGroup(group, stateGroup.States);
        Main.State.Init(group, new BotIdleStateExample());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Main.State.SwitchState(keyGroupStateBot, new BotAttackStateExample());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Main.State.SwitchState(keyGroupStateBot, new BotDieStateExample());
        }
    }
}
