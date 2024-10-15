using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using OSK;
using UnityEngine.Serialization;

public class BotStateExample : MonoBehaviour
{
    private string keyGroupStateBot = "BotStateExample";
    [SerializeField, ReadOnly] private GroupState groupState = null;

    private void Start()
    {
        groupState = Main.Fsm.CreateGroup(keyGroupStateBot);
        groupState.Add(new BotIdleStateExample());
        groupState.Add(new BotAttackStateExample());
        groupState.Add(new BotDieStateExample());

        groupState.Init(new BotIdleStateExample());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            groupState.Switch(new BotAttackStateExample());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            groupState.Switch(new BotDieStateExample());
        }

        var conditions = new bool[]
        {
            Input.GetKeyDown(KeyCode.O),
            Input.GetKey(KeyCode.I)
        };
        groupState.Switch(new BotIdleStateExample(), conditions);
    }
}