using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace OSK
{


    public class DotweenProviderSettings : ScriptableObject
    {
        [Header("Put the component types that need to be refreshed during preview here")]
        [Tooltip("When previewing in the editor, some components will not appear effective because they need to be refreshed in real time. " +
                 "Put these components here and the preview manager will refresh their instances")]
        public List<MonoScript> types = new List<MonoScript>();
        static DotweenProviderSettings m_Instance;
        static DotweenProviderSettings Instance
        {
            get
            {
                if (!m_Instance)
                {
                    m_Instance = LoadOrCreateMainAsset<DotweenProviderSettings>();
                }
                return m_Instance;
            }
        }

        void Awake()
        {
            // Known graphics components need to be refreshed when previewing, and first preset to the list
            types = new List<MonoScript>();
            types.Add(GetScriptAssetsByType(typeof(UnityEngine.UI.Graphic)));
        }

        internal static bool VerifyTargetNeedSetDirty(UnityEngine.Object target)
        {
            Predicate<MonoScript> predicate = v =>
            {
                return v.GetClass().IsAssignableFrom(target.GetType())
                          || v.GetClass() == target.GetType();
            };
            return Instance.types.Exists(predicate);
        }

        #region Assistance Function
        static T LoadOrCreateMainAsset<T>() where T : ScriptableObject
        {
            var ms = GetScriptAssetsByType(typeof(T));
            var path = AssetDatabase.GetAssetPath(ms);
            path = path.Substring(0, path.LastIndexOf("/"));
            path = Path.Combine(path, "Data");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var type = ms.GetClass();
            path = $"{path}/{type.Name}.asset";
            var asset = AssetDatabase.LoadMainAssetAtPath(path);
            if (!asset)
            {
                asset = CreateInstance(type);
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }
            return asset as T;
        }
        static MonoScript GetScriptAssetsByType(Type _type)
        {
            MonoScript monoScript = default;
            var scriptGUIDs = AssetDatabase.FindAssets($"t:script {_type.Name}");
            foreach (var scriptGUID in scriptGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
                monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                if (monoScript && monoScript.GetClass() == _type) break;
            }
            return monoScript;
        }
        #endregion
    }

}