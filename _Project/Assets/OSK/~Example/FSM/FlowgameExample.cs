using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowgameExample : MonoBehaviour
{
    private string groupName = "FollowGame";
    private void Start()
    {
        // Initialize the StateMachine with different state groups
        var group = Main.State.CreateGroup(groupName);

        // Add states to the group
        StateGroup stateGroup = new StateGroup(group);
        stateGroup.States.Add(new MenuStateExample());
        stateGroup.States.Add(new IngameStateExample());
        stateGroup.States.Add(new PauseStateExample());

        Main.State.AddListStateToGroup(group, stateGroup.States);
        
        // Set initial state
        Main.State.InitState(group, new MenuStateExample());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Main.State.SwitchState(groupName, new PauseStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Main.State.SwitchState(groupName, new IngameStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Main.State.SwitchState(groupName, new MenuStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Main.State.RemoveGroup("FollowGame");
        }
    }
 
    private void OnDestroy()
    {
        // Clean up the StateMachine
        Main.State.ExitState(groupName);
    }
}
