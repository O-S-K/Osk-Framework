using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowgameExample : MonoBehaviour
{
    private void Start()
    {
        // Initialize the StateMachine with different state groups
        var group = World.State.CreateGroup("FollowGame");

        // Add states to the group
        StateGroup stateGroup = new StateGroup(group);
        stateGroup.States.Add(new MenuStateExample());
        stateGroup.States.Add(new IngameStateExample());
        stateGroup.States.Add(new PauseStateExample());

        World.State.AddListStateToGroup(group, stateGroup.States);
        
        // Set initial state
        World.State.InitState(group, new MenuStateExample());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            World.State.ChangeState("FollowGame", new PauseStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            World.State.ChangeState("FollowGame", new IngameStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            World.State.ChangeState("FollowGame", new MenuStateExample());
        }
    }


    private void OnDestroy()
    {
        // Clean up the StateMachine
        World.State.ExitState("FollowGame");
    }
}
