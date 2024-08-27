using UnityEngine;

public class CubeFSM: StateMachine
{
    public IdleState idleState { get; private set; }
    public ChaseState chaseState { get; private set; }
    public AttackState attackState { get; private set; }

    [SerializeField] private MeshRenderer bodyMesh;
    [SerializeField] private Rigidbody rigidbody;

    private void Awake()
    {
        idleState = new IdleState(this, bodyMesh.material);
        chaseState = new ChaseState(this, bodyMesh.material, rigidbody);
        attackState = new AttackState(this, bodyMesh.material);
    }

    private void Start()
    {
        ChangeState(idleState);
    }

    private bool isPause = false;
    public override void Update()
    {
        base.Update();

        ExpamleCall();
    }

    private void ExpamleCall()
    {
        if (Input.GetKeyDown(KeyCode.P)) // pause
        {
            isPause = !isPause;
            SetPause(isPause);
        }
        if (Input.GetKeyDown(KeyCode.E)) // exit
        {
            ExitState();
        }
    }
}
