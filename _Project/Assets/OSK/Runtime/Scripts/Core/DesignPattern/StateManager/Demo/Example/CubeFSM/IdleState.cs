using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IStateMachine
{
    private CubeFSM cubeFSM;
    private Material meshBody;

    public IdleState(CubeFSM cubeFPS, Material meshBody)
    {
        this.cubeFSM = cubeFPS;
        this.meshBody = meshBody;
    }

    public void Enter()
    {
        meshBody.color = Color.green;
        cubeFSM.transform.position = new Vector3(0, 0, -5);
    }

    public void Exit()
    {
    }

    public void FixedTick()
    {
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cubeFSM.ChangeState(cubeFSM.chaseState);
        }
    }
}
