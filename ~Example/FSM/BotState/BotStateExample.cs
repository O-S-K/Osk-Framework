using UnityEngine;
using OSK;
using System;

public class BotStateExample : MonoBehaviour
{
    private string keyGroupStateBot = "BotStateExample";
    private StateMachine state;
    private Timer time;


    private bool isIdle;
    private bool isAttack;
    private bool isDie;

    public string currentState;

    private void Start()
    {
        time = Main.Time.CreateLoops(this);
        time.OnStart += OnEnter;
        time.OnTick += OnTick;
        time.OnFixedTick += OnFixedTick;
        time.OnCompleted += OnExit;
        
        time.OnStart?.Invoke();
    }

    private void OnDestroy()
    {
        time.OnStart -= OnEnter;
        time.OnTick -= OnTick;
        time.OnFixedTick -= OnFixedTick;
        time.OnCompleted -= OnExit;
    }


    private void OnEnter()
    {
        Debug.Log("BotStateExample OnEnter");
        state = Main.Fsm.Create(keyGroupStateBot);

        var idle = new BotIdleStateExample(this);
        var attack = new BotAttackStateExample(this);
        var die = new BotDieStateExample(this);

        state.Add(new IState[] { idle, attack, die });

        state.At(idle, attack, () => isAttack);
        state.At(attack, idle, () => isIdle);
        state.Any(die, () => isDie);

        isIdle = true;
        state.Init(idle);
    }

    private void OnTick()
    {
        state?.Tick();
        GetInput();
        if (state != null)
            currentState = state.GetCurrentState().ToString();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isIdle = true;
            isAttack = false;
            isDie = false;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            isIdle = false;
            isAttack = true;
            isDie = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            isIdle = false;
            isAttack = false;
            isDie = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gameObject.SetActive(false);
        } 
    }

    private void OnFixedTick()
    {
        state?.FixedTick();
    }

    private void OnExit()
    {
        Debug.Log("BotStateExample OnExit");
        Main.Fsm.Exit(keyGroupStateBot);
    }
}