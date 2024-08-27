using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IStateMachine
{
    private float speedChase = 100;
    private float speedRotation = 100;

    private CubeFSM cubeFPS;
    private Material meshBody;
    private Rigidbody rigidbody;

    public ChaseState(CubeFSM cubeFPS, Material meshBody, Rigidbody rigidbody)
    {
        this.cubeFPS = cubeFPS;
        this.meshBody = meshBody;
        this.rigidbody = rigidbody;
    }

    public void Enter()
    {
        meshBody.color = Color.black;
    }

    public void Exit()
    {
    }

    public void FixedTick()
    {
        float distanceFromTarget = Vector3.Distance(Vector3.zero, rigidbody.position);
        if (distanceFromTarget < .1f)
        {
            cubeFPS.ChangeState(cubeFPS.attackState);
        }
        else
        {
            RotateTowardsTarget();
            MoveTowardsTarget();
        }
    }

    public void Tick()
    {
    } 

    private void RotateTowardsTarget()
    {
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.zero - rigidbody.position);
        lookRotation = Quaternion.Slerp(rigidbody.rotation, lookRotation, speedRotation * Time.fixedDeltaTime);
        rigidbody.MoveRotation(lookRotation);
    }

    private void MoveTowardsTarget()
    {
        Vector3 moveOffset = cubeFPS.transform.forward * speedChase * Time.fixedDeltaTime;
        rigidbody.velocity = moveOffset;
    }
}
