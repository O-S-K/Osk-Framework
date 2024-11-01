using System;
using UnityEngine;
using OSK;

public class FlowgameExample : MonoBehaviour
{
    private string keyFlowgame = "FlowgameExample";
    private StateMachine state;
    public string currentState;

    private void Start()
    {
        state = Main.Fsm.Create(keyFlowgame);
        
        var menuUI = new MenuStateExample(this);
        var inGameUI = new IngameStateExample(this);
        var pauseUI = new PauseStateExample(this);
        
        state.Add(new IState[] { menuUI, inGameUI, pauseUI });
        
        state.At(menuUI, inGameUI, () => Input.GetKeyDown(KeyCode.A));
        state.At(inGameUI, menuUI, () => Input.GetKeyDown(KeyCode.D));
        
        state.Any(pauseUI, () => Input.GetKeyDown(KeyCode.Space));
        
        state.At(pauseUI, menuUI, () => Input.GetKeyDown(KeyCode.C));
        state.At(pauseUI, inGameUI, () => Input.GetKeyDown(KeyCode.V));
       
        state.Init(menuUI);
    }
    
    private void Update()
    {
        state?.Tick();
        currentState = state?.GetCurrentState().ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Main.Fsm.Remove(keyFlowgame);
        }
    }
    
    private void FixedUpdate()
    {
        state?.FixedTick();
    }
}