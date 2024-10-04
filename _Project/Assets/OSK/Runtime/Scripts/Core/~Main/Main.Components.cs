using System;
using OSK;
using UnityEngine;

public partial class Main : MonoBehaviour
{
    public static Observer Observer { get; private set; }
    public static EventBus EventBus { get; private set; }
    public static StateMachine State { get; private set; }
    public static PoolManager Pool { get; private set; }
    public static CommandManager Command { get; private set; }
    public static SceneManager Scene { get; private set; }
    public static ResourceManager Res { get; private set; }
    public static SaveManager Save { get; private set; }
    public static NetworkManager Network { get; private set; }
    public static WebRequestManager WebRequest { get; private set; }
    public static GameConfigs Configs { get; private set; }
    public static UIManager UI { get; private set; }
    public static SoundManager Sound { get; private set; }
    public static LocalizationManager Localization { get; private set; }
    public static EntityManager Entity { get; private set; }
    public static TimeManager Time { get; private set; }
    public static AbilityManager Ability { get; private set; }
    public static Performance Performance { get; private set; }
    public static NativeManager Native { get; private set; }

    private static void InitComponents()
    {
        Observer = Main.GetFrameworkComponent<Observer>();
        EventBus = Main.GetFrameworkComponent<EventBus>();
        State = Main.GetFrameworkComponent<StateMachine>();
        Pool = Main.GetFrameworkComponent<PoolManager>();
        Command = Main.GetFrameworkComponent<CommandManager>();
        Scene = Main.GetFrameworkComponent<SceneManager>();
        Res = Main.GetFrameworkComponent<ResourceManager>();
        Save = Main.GetFrameworkComponent<SaveManager>();
        Network = Main.GetFrameworkComponent<NetworkManager>();
        WebRequest = Main.GetFrameworkComponent<WebRequestManager>();
        Configs = Main.GetFrameworkComponent<GameConfigs>();
        UI = Main.GetFrameworkComponent<UIManager>();
        Sound = Main.GetFrameworkComponent<SoundManager>();
        Localization = Main.GetFrameworkComponent<LocalizationManager>();
        Entity = Main.GetFrameworkComponent<EntityManager>();
        Time = Main.GetFrameworkComponent<TimeManager>();
        Ability = Main.GetFrameworkComponent<AbilityManager>();
        Performance = Main.GetFrameworkComponent<Performance>();
        Native = Main.GetFrameworkComponent<NativeManager>();
        OSK.Logg.Log("Init Components Done!");
    }
}