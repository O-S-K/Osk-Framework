using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStateExample : MonoBehaviour
{
    private void Start()
    {
        var group= World.State.CreateGroup("BotStateExample");
        
        StateGroup stateGroup = new StateGroup(group);
        stateGroup.States.Add(new BotIdleStateExample());
        stateGroup.States.Add(new BotAttackStateExample());
        stateGroup.States.Add(new BotDieStateExample());
        
        World.State.AddListStateToGroup(group, stateGroup.States);
        World.State.InitState(group, new BotIdleStateExample());
    }
}
