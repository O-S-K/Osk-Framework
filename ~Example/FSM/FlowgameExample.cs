using UnityEngine;
using OSK;
using CustomInspector;

public class FlowgameExample : MonoBehaviour
{
    private string keyGroupName = "FollowGame";
    [SerializeField, ReadOnly] private GroupState groupState = null;

    private void Start()
    {
        groupState = Main.Fsm.CreateGroup(keyGroupName);

        // Add states to group
        groupState.Add(new PauseStateExample());
        groupState.Add(new IngameStateExample());
        groupState.Add(new MenuStateExample());

        // Set initial state
        groupState.Init(new MenuStateExample());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            groupState.Switch(new PauseStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            groupState.Switch(new IngameStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            groupState.Switch(new MenuStateExample());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Main.Fsm.RemoveGroup(keyGroupName);
        }
    }

    private void OnDestroy()
    {
        Main.Fsm.Exit(keyGroupName);
    }
}