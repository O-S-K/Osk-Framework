using System;
using OSK;
using UnityEngine;

public partial class World : MonoBehaviour
{
    public static Observer Observer { get; private set; }
    public static  EventBus EventBus { get; private set; }
    public static  StateMachine State { get; private set; }
    public static  PoolManager Pool { get; private set; }
    public static  SceneManager Scene { get; private set; }
    public static  ResourceManager Res { get; private set; }
    public static  DataManager Data { get; private set; }
    public static  OSK.Logger Log { get; private set; }
    public static  NetworkManager Network { get; private set; }
    public static  WebRequestManager WebRequest { get; private set; }
    public static  GameConfigs Configs { get; private set; }
    public static  UIManager UI { get; private set; }
    public static  SoundManager Sound { get; private set; }
    public static  LocalizationManager Localization { get; private set; }
    public static  EntityManager Entity { get; private set; }
    public static  TimeManager Time { get; private set; }
    
    private static void InitComponents()
    {
        Debug.Log("InitComponents");
        Observer =  World.GetFrameworkComponent<Observer>();
        EventBus = World.GetFrameworkComponent<EventBus>();
        State = World.GetFrameworkComponent<StateMachine>();
        Pool = World.GetFrameworkComponent<PoolManager>();
        Scene = World.GetFrameworkComponent<SceneManager>();
        Res = World.GetFrameworkComponent<ResourceManager>();
        Data = World.GetFrameworkComponent<DataManager>();
        Log = World.GetFrameworkComponent<OSK.Logger>();
        Network = World.GetFrameworkComponent<NetworkManager>();
        WebRequest = World.GetFrameworkComponent<WebRequestManager>();
        Configs = World.GetFrameworkComponent<GameConfigs>();
        UI = World.GetFrameworkComponent<UIManager>();
        Sound = World.GetFrameworkComponent<SoundManager>();
        Localization = World.GetFrameworkComponent<LocalizationManager>();
        Entity = World.GetFrameworkComponent<EntityManager>();
        Time = World.GetFrameworkComponent<TimeManager>();
    }
    
}