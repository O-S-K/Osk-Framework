using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IStateMachine
{
    private CubeFSM cubeFSM;
    private Material meshBody;
    private bool canAttack = true;
    private float timeNextState = 1;

    public AttackState(CubeFSM cubeFSM, Material meshBody)
    {
        this.cubeFSM = cubeFSM;
        this.meshBody = meshBody;
    }

    public void Enter()
    {
        meshBody.color = Color.red;
        canAttack = true;
        timeNextState = 1;
    }

    public void Exit()
    {
    }

    public void FixedTick()
    {
    }

    public void Tick()
    {
        if (canAttack)
        {
            canAttack = false;
        }
        else
        {
            if(timeNextState > 0)
            {
                timeNextState -= Time.deltaTime;
            }
            else
            {
                timeNextState = 1;
                cubeFSM.ChangeState(cubeFSM.idleState);
            }
        }
    } 
}
