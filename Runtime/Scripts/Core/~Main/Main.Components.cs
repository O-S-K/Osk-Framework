using System;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    /// <summary>
    /// Main class of the framework, contains all the components of the framework.
    /// </summary>
    public partial class Main
    {
        public static MonoManager Mono { get; private set; }
        public static ServiceLocatorManager Service { get; private set; }
        public static ObserverManager Observer { get; private set; }
        public static EventBusManager Event { get; private set; }
        public static FSMManager Fsm { get; private set; }
        public static PoolManager Pool { get; private set; }
        public static CommandManager Command { get; private set; }
        public static SceneManager Scene { get; private set; }
        public static ResourceManager Res { get; private set; }
        public static SaveManager Save { get; private set; }
        public static DataManager Data { get; private set; }
        public static NetworkManager Network { get; private set; }
        public static WebRequestManager WebRequest { get; private set; }
        public static GameConfigsManager Configs { get; private set; }
        public static UIManager UI { get; private set; }
        public static SoundManager Sound { get; private set; }
        public static LocalizationManager Localization { get; private set; }
        public static EntityManager Entity { get; private set; }
        public static TimeManager Time { get; private set; }
        public static NativeManager Native { get; private set; }
        public static BlackboardManager Blackboard { get; private set; }


        public MainModules mainModules;
        public ConfigInit configInit;
        public bool isDestroyingOnLoad = false;

        private void Awake()
        {
            if (isDestroyingOnLoad)
                DontDestroyOnLoad(gameObject);
            InitModules();
            InitDataComponents();
        }

        public void InitModules()
        {
            foreach (ModuleType module in Enum.GetValues(typeof(ModuleType)))
            {
                if (module != ModuleType.None && (mainModules.Modules & module) != 0)
                {
                    string moduleName = module.ToString();

                    GameObject newObject = new GameObject(moduleName);
                    newObject.transform.SetParent(transform);
                    var componentType = mainModules.GetComponentType(moduleName);
                    if (componentType != null)
                    {
                        var _module = newObject.AddComponent(componentType) as GameFrameworkComponent;
                        if (_module is MonoManager manager) Mono = manager;
                        else if (_module is ServiceLocatorManager locator) Service = locator;
                        else if (_module is ObserverManager observer) Observer = observer;
                        else if (_module is EventBusManager eventBus) Event = eventBus;
                        else if (_module is FSMManager fsm) Fsm = fsm;
                        else if (_module is PoolManager pool) Pool = pool;
                        else if (_module is CommandManager command) Command = command;
                        else if (_module is SceneManager scene) Scene = scene;
                        else if (_module is ResourceManager res) Res = res;
                        else if (_module is SaveManager save) Save = save;
                        else if (_module is DataManager data) Data = data;
                        else if (_module is NetworkManager network) Network = network;
                        else if (_module is WebRequestManager webRequest) WebRequest = webRequest;
                        else if (_module is GameConfigsManager configs) Configs = configs;
                        else if (_module is UIManager ui) UI = ui;
                        else if (_module is SoundManager sound) Sound = sound;
                        else if (_module is LocalizationManager localization) Localization = localization;
                        else if (_module is EntityManager entity) Entity = entity;
                        else if (_module is TimeManager time) Time = time;
                        else if (_module is NativeManager native) Native = native;
                        else if (_module is BlackboardManager blackboard) Blackboard = blackboard; 
                        else
                        {
                            Logg.LogError($"Module {_module} not found");
                        }
                    }
                    else
                    {
                        Logg.LogError($"Module {moduleName} not found");
                    }
                }
            }
        }

        private void InitDataComponents()
        {
            var current = SGameFrameworkComponents.First;
            while (current != null)
            {
                current.Value.OnInit();
                current = current.Next;
            }

            OSK.Logg.Log("Init Data Components Done!");
        }
    }
}