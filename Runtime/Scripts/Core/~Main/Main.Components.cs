using System;
using OSK;
using UnityEngine;

namespace OSK
{
    public partial class Main
    {
        public static Observer Observer { get; private set; }
        public static EventBus EventBus { get; private set; }
        public static FSMManager Fsm { get; private set; }
        public static PoolManager Pool { get; private set; }
        public static CommandManager Command { get; private set; }
        public static SceneManager Scene { get; private set; }
        public static ResourceManager Res { get; private set; }
        public static SaveManager Save { get; private set; }
        public static DataManager Data { get; private set; }
        public static NetworkManager Network { get; private set; }
        public static WebRequestManager WebRequest { get; private set; }
        public static GameConfigs Configs { get; private set; }
        public static UIManager UI { get; private set; }
        public static SoundManager Sound { get; private set; }
        public static LocalizationManager Localization { get; private set; }
        public static EntityManager Entity { get; private set; }
        public static TimeManager Time { get; private set; }
        public static NativeManager Native { get; private set; }


        public bool isDestroyingOnLoad = false;

        public void Awake()
        {
            if (isDestroyingOnLoad)
                DontDestroyOnLoad(gameObject);
            InitComponents();
        }

        public static void InitComponents()
        {
            Save = Main.GetFrameworkComponent<SaveManager>();
            Data = Main.GetFrameworkComponent<DataManager>();
            Configs = Main.GetFrameworkComponent<GameConfigs>();
            Localization = Main.GetFrameworkComponent<LocalizationManager>();
            Time = Main.GetFrameworkComponent<TimeManager>();

            Pool = Main.GetFrameworkComponent<PoolManager>();
            Observer = Main.GetFrameworkComponent<Observer>();
            EventBus = Main.GetFrameworkComponent<EventBus>();
            Fsm = Main.GetFrameworkComponent<FSMManager>();
            Command = Main.GetFrameworkComponent<CommandManager>();

            Scene = Main.GetFrameworkComponent<SceneManager>();
            Res = Main.GetFrameworkComponent<ResourceManager>();
            Network = Main.GetFrameworkComponent<NetworkManager>();
            WebRequest = Main.GetFrameworkComponent<WebRequestManager>();
            UI = Main.GetFrameworkComponent<UIManager>();
            Sound = Main.GetFrameworkComponent<SoundManager>();
            Entity = Main.GetFrameworkComponent<EntityManager>();
            Native = Main.GetFrameworkComponent<NativeManager>();
            OSK.Logg.Log("Init Components Done!");
        }
    }
}