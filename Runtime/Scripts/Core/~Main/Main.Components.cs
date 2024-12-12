using UnityEngine;

namespace OSK
{
    /// <summary>
    /// Main class of the framework, contains all the components of the framework.
    /// </summary>
    public partial class Main
    {
        public static MonoManager Mono { get; private set; }
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
 
        private void Awake()
        {
            if (isDestroyingOnLoad)
                DontDestroyOnLoad(gameObject);
            InitComponents();
            InitDataComponents();
            CheckNullComponents();
        }

        private static void InitComponents()
        {
            Mono         = GetModule<MonoManager>();
            Configs      = GetModule<GameConfigs>();
            Save         = GetModule<SaveManager>();
            Data         = GetModule<DataManager>();
            Localization = GetModule<LocalizationManager>();
            Time         = GetModule<TimeManager>();

            Service      = GetModule<ServiceLocator>();
            Pool         = GetModule<PoolManager>();
            Observer     = GetModule<Observer>();
            EventBus     = GetModule<EventBus>();
            Fsm          = GetModule<FSMManager>();
            Command      = GetModule<CommandManager>();

            Scene        = GetModule<SceneManager>();
            Res          = GetModule<ResourceManager>();
            Network      = GetModule<NetworkManager>();
            WebRequest   = GetModule<WebRequestManager>();
            UI           = GetModule<UIManager>();
            Sound        = GetModule<SoundManager>();
            Entity       = GetModule<EntityManager>();
            Native       = GetModule<NativeManager>();
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
             Logg.Log("Check Null Components...");
             //Logg.CheckNullRef(DI == null, "Injector");
             Logg.CheckNullRef(Mono== null, "Mono");
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