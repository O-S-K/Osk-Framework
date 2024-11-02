using System;
using OSK;
using UnityEngine;

namespace OSK
{
    /// <summary>
    /// Main class of the framework, contains all the components of the framework.
    /// </summary>
    public partial class Main
    {
        public static ServiceLocator Service { get; private set; }
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
            InitDataComponents();
            CheckNullComponents();
        }

        private static void InitComponents()
        {
            Configs      = Main.GetModule<GameConfigs>();
            Save         = Main.GetModule<SaveManager>();
            Data         = Main.GetModule<DataManager>();
            Localization = Main.GetModule<LocalizationManager>();
            Time         = Main.GetModule<TimeManager>();

            Service      = Main.GetModule<ServiceLocator>();
            Pool         = Main.GetModule<PoolManager>();
            Observer     = Main.GetModule<Observer>();
            EventBus     = Main.GetModule<EventBus>();
            Fsm          = Main.GetModule<FSMManager>();
            Command      = Main.GetModule<CommandManager>();

            Scene        = Main.GetModule<SceneManager>();
            Res          = Main.GetModule<ResourceManager>();
            Network      = Main.GetModule<NetworkManager>();
            WebRequest   = Main.GetModule<WebRequestManager>();
            UI           = Main.GetModule<UIManager>();
            Sound        = Main.GetModule<SoundManager>();
            Entity       = Main.GetModule<EntityManager>();
            Native       = Main.GetModule<NativeManager>();
            
            OSK.Logg.Log("Init Components Done!");
        }
         
        public static void InitDataComponents()
        {
            var current = SGameFrameworkComponents.First;
            while (current != null)
            {
                current.Value.OnInit();
                current = current.Next;
            }
            
            OSK.Logg.Log("Init Data Components Done!");
        }
        
        public void CheckNullComponents()
        {
             Logg.CheckNullRef(Configs == null, "Configs");
             Logg.CheckNullRef(Save== null, "Save");
             Logg.CheckNullRef(Data== null, "Data");
             Logg.CheckNullRef(Localization== null, "Localization");
             Logg.CheckNullRef(Time== null, "Time");
             Logg.CheckNullRef(Service== null, "Service");
             Logg.CheckNullRef(Pool== null, "Pool");
             Logg.CheckNullRef(Observer== null, "Observer");
             Logg.CheckNullRef(EventBus== null, "EventBus");
             Logg.CheckNullRef(Fsm== null, "Fsm");
             Logg.CheckNullRef(Command== null, "Command");
             Logg.CheckNullRef(Scene== null, "Scene");
             Logg.CheckNullRef(Res== null, "Res");
             Logg.CheckNullRef(Network== null, "Network");
             Logg.CheckNullRef(WebRequest== null, "WebRequest");
             Logg.CheckNullRef(UI== null, "UI");
             Logg.CheckNullRef(Sound== null, "Sound");
             Logg.CheckNullRef(Entity== null, "Entity");
             Logg.CheckNullRef(Native== null, "Native");
             Logg.Log("Check Null Components Done!");
        }
    }
}