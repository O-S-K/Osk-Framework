using DG.DOTweenEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace OSK
{
    public static class DotweenPreviewManager
    {
        static List<Tweener> tweeners = new List<Tweener>();
        [InitializeOnLoadMethod]
        static void ModelImporterAvatarSetup()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode && DOTweenEditorPreview.isPreviewing)
            {
                tweeners.Clear();
                DOTweenEditorPreview.Stop(true);
            }
        }
        #region Extension Methods
        public static void StopPreview(this IDoTweenProviderBehaviours provider)
        {
            if (null == provider) return;
            TweenerPostProcess(provider);
        }
        public static void StartPreview(this IDoTweenProviderBehaviours provider, TweenCallback OnStart= null,TweenCallback OnUpdate = null)
        {
            if (null == provider) return;
            provider.Play();
            var tweener = provider.Tweener;
            if (!tweeners.Contains(tweener))
            {
                tweeners.Add(tweener);
            }
            if (!DOTweenEditorPreview.isPreviewing)
            {
                DOTweenEditorPreview.Start();
            }
            DOTweenEditorPreview.PrepareTweenForPreview(tweener);
            // Register callback, be sure to add the listener at the end
            tweener.OnComplete(() => TweenerPostProcess(provider))
            .OnStart(OnStart)
            .OnUpdate(OnUpdate);
        }
        
        public static void StopPreview(this DotweenProviderManager manager)
        {
            if (null == manager) return;
            foreach (var provider in manager.Providers)
            {
                provider.StopPreview();
            }
        }
        public static void StartPreview(this DotweenProviderManager manager)
        {
            if (null == manager) return;
            Debug.Assert(manager.Providers.Count != 0, "Manager Not found under its child nodes, Dotween Provider");
            foreach (var provider in manager.Providers)
            {
                provider.StopPreview();
                provider.StartPreview();
            }
        } 
      
        public static bool IsPreviewing(this DotweenProviderManager manager)
        {
            return manager.Providers.Any(v => v.IsPreviewing());
        }
        
        public static bool IsPreviewing(this IDoTweenProviderBehaviours provider)
        {
            return !EditorApplication.isPlayingOrWillChangePlaymode 
                   &&null != provider 
                   && null != provider.Tweener 
                   && provider.Tweener.active
                   && provider.Tweener.IsPlaying() 
                   && tweeners.Contains(provider.Tweener);
        }  
        #endregion

        #region Assistance Funtion
        static void SetHideFlags(this IDoTweenProviderBehaviours provider, HideFlags hideFlags)
        {
            ((Component)provider).gameObject.hideFlags = hideFlags;
        }
        static void Reset(this Tweener tween)
        {
            if (null == tween) return;
            try
            {
                if (IsFrom(tween))
                {
                    tween.Complete();
                }
                else
                {
                    tween.Rewind();
                }
            }
            catch { }
        }

        private static bool IsFrom(Tweener tween)
        {
            var info = tween.GetType()
                .GetField("isFrom", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(tween);
            return (bool)info;
        }

        private static void UpdateManagerState()
        {
            ValidateTween();
            if (tweeners.Count == 0)
            {
                DOTweenEditorPreview.Stop();
            }
        }

        private static void ValidateTween()
        {
            for (int num = tweeners.Count - 1; num > -1; num--)
            {
                if (tweeners[num] == null || !tweeners[num].active)
                {
                    tweeners.RemoveAt(num);
                }
            }
        }

        private static void TweenerPostProcess(IDoTweenProviderBehaviours provider)
        {
            var tweener = provider.Tweener;
            if (tweeners.Contains(tweener))
            {
                tweener.Reset(); //Reset Dotween's changes first
                provider.Stop(); //Then call the user stop logic, which may contain the component's own reset logic
                EditorUtility.SetDirty(((Component)provider).gameObject); //Finally reset the dirty flag to avoid reset data loss
                tweeners.Remove(tweener);
                UpdateManagerState();
            }
        }

        #endregion
    }
}