using OSK;
using UnityEngine;

public class BotAttackStateExample : IState
{
    public string StateName => "BotAttackStateExample";
    private BotStateExample botStateExample;
    
    public BotAttackStateExample(BotStateExample botStateExample)
    {
        this.botStateExample = botStateExample;
    }

    public void OnEnter()
    {
        Debug.Log("Attack");
    }

    public void Tick()
    {
    }

    public void FixedTick()
    {
    }

    public void OnExit()
    {
    } 
}