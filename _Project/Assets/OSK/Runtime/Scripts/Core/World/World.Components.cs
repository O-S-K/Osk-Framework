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
    public static SceneManager Scene { get; private set; }
    public static ResourceManager Res { get; private set; }
    public static DataManager Data { get; private set; }
    public static OSK.Logger Log { get; private set; }

    public static NetworkManager Network { get; private set; }
    public static WebRequestManager WebRequest { get; private set; }

    public static GameConfigs Configs { get; private set; }
    public static UIManager UI { get; private set; }

    public static SoundManager Sound { get; private set; }

    public static LocalizationManager Localization { get; private set; }

    public static EntityManager Entity { get; private set; }
    public static TimeManager Time { get; private set; }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Init()
    {
        Observer = World.Get<Observer>();
        EventBus = World.Get<EventBus>();
        Pool = World.Get<PoolManager>();
        Scene = World.Get<SceneManager>();
        State = World.Get<StateMachine>();

        Data = World.Get<DataManager>();
        Res = World.Get<ResourceManager>();
        Network = World.Get<NetworkManager>();
        WebRequest = World.Get<WebRequestManager>();
        Log = World.Get<OSK.Logger>();

        Configs = World.Get<GameConfigs>();
        UI = World.Get<UIManager>();
        Sound = World.Get<SoundManager>();
        Localization = World.Get<LocalizationManager>();
        Entity = World.Get<EntityManager>();
        Time = World.Get<TimeManager>();
    }
}