using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public partial class World : MonoBehaviour
{
    public static Observer Observer { get; private set; }
    public static EventBus EventBus { get; private set; }
    public static StateMachine State { get; private set; }
    public static PoolManager Pool { get; private set; }
    public static PlayerPrefsSystem PrefData { get; private set; }
    public static  SceneManager Scene { get; private set; }
    public static FileSystem File { get; private set; }
    public static JsonSystem Json { get; private set; }
    public static ResourceManager Res { get; private set; }
    public static OSK.Logger Log { get; private set; }
    
    public static NetworkManager Network { get; private set; }
    public static WebRequestManager WebRequest { get; private set; }
    
    public static GameConfigs Configs { get; private set; }
    public static UIManager UI { get; private set; }
    
    public static SoundManager Sound { get; private set; }
    
    public static LocalizationManager Localization { get; private set; }
    
    public static EntityManager Entities { get; private set; }

    private void Init()
    {
        Observer = transform.GetComponentInChildren<Observer>();
        EventBus = transform.GetComponentInChildren<EventBus>();
        Pool = transform.GetComponentInChildren<PoolManager>();
        Scene = transform.GetComponentInChildren<SceneManager>();
        State = transform.GetComponentInChildren<StateMachine>();
        
        Res = transform.GetComponentInChildren<ResourceManager>();
        PrefData = transform.GetComponentInChildren<PlayerPrefsSystem>();
        File = transform.GetComponentInChildren<FileSystem>();
        Json = transform.GetComponentInChildren<JsonSystem>();
        
        Network = transform.GetComponentInChildren<NetworkManager>();
        WebRequest = transform.GetComponentInChildren<WebRequestManager>();
        Log = transform.GetComponentInChildren<OSK.Logger>();
        
        Configs = transform.GetComponentInChildren<GameConfigs>();
        UI = transform.GetComponentInChildren<UIManager>();
        Sound = transform.GetComponentInChildren<SoundManager>();
        Localization = transform.GetComponentInChildren<LocalizationManager>();
        Entities = transform.GetComponentInChildren<EntityManager>();
        
    }
}