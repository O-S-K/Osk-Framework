using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    [CreateAssetMenu(menuName = "OSK/MainModules")]
    public class MainModules : ScriptableObject
    {
        private Dictionary<string, Type> componentTypeCache = new Dictionary<string, Type>();

        [Title("Modules Selection")]
        [EnumToggleButtons]
        [SerializeField] private ModuleType _modules; 
        public ModuleType Modules => _modules;

        public System.Type GetComponentType(string moduleName)
        {
            if (componentTypeCache.ContainsKey(moduleName))
                return componentTypeCache[moduleName];

            string fullTypeName = "OSK." + moduleName;

            var componentType = System.Type.GetType(fullTypeName);
            if (componentType != null)
            {
                componentTypeCache[moduleName] = componentType;
            }

            return componentType;
        }

        [Space(10)] [Title("Setup Modules")]
        [ReadOnly]
        public string title = "Select the modules you want to enable in the game.";

        [Button]
        private void EnableAllModule()
        {
            _modules = (ModuleType)~0;
            Debug.Log("All modules have been selected.");
        }

        [Button]
        private void DisableAllModule()
        {
            _modules = 0;
            Debug.Log("All modules have been deselected.");
        }
    }

    [System.Flags]
    public enum ModuleType
    {
        None = 0,
        MonoManager = 1 << 0,
        ServiceLocatorManager = 1 << 1,
        ObserverManager = 1 << 2,
        EventBusManager = 1 << 3,
        FSMManager = 1 << 4,
        PoolManager = 1 << 5,
        CommandManager = 1 << 6,
        SceneManager = 1 << 7,
        ResourceManager = 1 << 8,
        SaveManager = 1 << 9,
        DataManager = 1 << 10,
        NetworkManager = 1 << 11,
        WebRequestManager = 1 << 12,
        GameConfigsManager = 1 << 13,
        UIManager = 1 << 14,
        SoundManager = 1 << 15,
        LocalizationManager = 1 << 16,
        EntityManager = 1 << 17,
        TimeManager = 1 << 18,
        NativeManager = 1 << 19,
        BlackboardManager = 1 << 20,
    }
}