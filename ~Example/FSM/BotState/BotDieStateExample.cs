using OSK;

public class BotDieStateExample : IState
{
    public string StateName => "BotDieStateExample";
    
    private BotStateExample botStateExample;
    
    public BotDieStateExample(BotStateExample botStateExample)
    {
        this.botStateExample = botStateExample;
    }

    public void OnEnter()
    {
        UnityEngine.Debug.Log("Die");
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