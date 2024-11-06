using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    [CustomEditor(typeof(DoTweenBaseProvider), true)]
    public class DoTweenBaseProviderEditor : Editor
    {
        public DoTweenBaseProvider provider;
        bool fd0 = true;
        bool fd1 = true;
        GUIStyle bt;
        GUIStyle fds;
        SerializedProperty loopcount;
        SerializedProperty duration;
        float _previewTime = 0;
        bool isPreview;

        public override void OnInspectorGUI()
        {
            #region Init GUIStyle

            if (null == bt) bt = new GUIStyle(EditorStyles.miniButtonRight) { fixedWidth = 100, richText = true };
            if (null == fds)
                fds = new GUIStyle(EditorStyles.foldoutHeader) { fontStyle = FontStyle.Normal, fontSize = 14 };
            fds.onNormal.textColor = Color.white;

            #endregion

            GUI.enabled = EditorApplication.isPlaying || !provider.IsPreviewing();
            serializedObject.Update();

            #region Data Verification

            loopcount = loopcount ?? serializedObject.FindProperty("loopcount");
            loopcount.intValue = loopcount.intValue < -1 ? -1 : loopcount.intValue;
            duration = duration ?? serializedObject.FindProperty("duration");
            duration.floatValue = duration.floatValue <= 0 ? 0.1f : duration.floatValue;

            #endregion

            var itr = serializedObject.GetIterator();
            itr.NextVisible(true);
            fd0 = EditorGUILayout.Foldout(fd0, "General Parameters", fds);
            if (fd0)
            {
                HorizontalLine();
                while (itr.NextVisible(false))
                {
                    if (itr.name != "loopType" || provider.loopcount != 0 && provider.loopcount != 1)
                    {
                        EditorGUILayout.PropertyField(itr, true);
                    }

                    if (itr.name == "ease") break;
                }
            }

            fd1 = EditorGUILayout.Foldout(fd1, " Provider Parameter ", fds);
            if (fd1)
            {
                HorizontalLine();
                while (itr.NextVisible(false))
                {
                    if (fd0 || IsPropertyDrawRequired(itr))
                    {
                        EditorGUILayout.PropertyField(itr, true);
                    }
                }
            }

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }

            bool isTweening = !EditorApplication.isPlayingOrWillChangePlaymode && provider.IsPreviewing();
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode && provider.gameObject.activeInHierarchy;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();

            if (!isPreview)
            {
                if (GUILayout.Button(isTweening ? "Stop preview" : "Start preview"))
                {
                    if (isTweening)
                    {
                        provider.StopPreview();
                    }
                    else
                    {
                        provider.StopPreview();
                        provider.StartPreview(OnTweenerStart, OnTweenerUpdating);
                    }
                }

                var providers = provider.GetComponents<DoTweenBaseProvider>();
                if (null != providers && providers.Length > 1)
                {
                    // if(isPreview)
                    //     return;

                    var anyispreviewing = providers.Any(v => v.IsPreviewing());
                    var label = anyispreviewing ? "<color=red>Stop All</color>" : "Preview all";
                    if (GUILayout.Button(label, bt))
                    {
                        if (anyispreviewing)
                        {
                            foreach (var item in providers)
                            {
                                item.StopPreview();
                            }
                        }
                        else
                        {
                            foreach (var item in providers)
                            {
                                item.StopPreview();
                                item.StartPreview(() => OnTweenerStart(providers), () => OnTweenerUpdating(item));
                            }
                        }
                    }
                }
            }

            GUILayout.EndHorizontal();
            isPreview = GUILayout.Toggle(isPreview, "Preview Control");
            if (isPreview)
            {
                if (!_isPlaying)
                {
                    _isPlaying = true;
                    _isCancelPreviewing = false;
                    provider.StopPreview();
                    provider.StartPreview(OnTweenerStart, OnTweenerUpdating);
                }

                EditorGUILayout.LabelField("Preview Control");
                _previewTime = EditorGUILayout.Slider("Timeline", _previewTime, 0, provider.GetDuration() - 0.0001f);
                if (EditorGUI.EndChangeCheck())
                {
                    provider.Preview(_previewTime);
                }

                provider.SetIsPreview(true);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Stop preview"))
                {
                    _previewTime = 0;
                    _isPlaying = false;
                    provider.SetIsPreview(false);
                    if (!_isCancelPreviewing)
                    {
                        _isCancelPreviewing = true;
                        provider.StopPreview();
                    }
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                _isPlaying = false;
                provider.SetIsPreview(false);
                if (!_isCancelPreviewing)
                {
                    _isCancelPreviewing = true;
                    provider.StopPreview();
                }
            }
        }

        bool _isPlaying;
        bool _isCancelPreviewing;
        bool isTweenerTargetNeedSetDirty = false;

        public virtual void OnTweenerStart(DoTweenBaseProvider[] providers)
        {
            isTweenerTargetNeedSetDirty =
                providers.Any(v => DotweenProviderSettings.VerifyTargetNeedSetDirty(v.target));
        }

        public virtual void OnTweenerStart()
        {
            isTweenerTargetNeedSetDirty = DotweenProviderSettings.VerifyTargetNeedSetDirty(provider.target);
        }

        public virtual void OnTweenerUpdating()
        {
            //If you want to preview TextMeshPro UGUI and Text, you must refresh
            //For other types, please set in DotweenProviderSettings
            if (isTweenerTargetNeedSetDirty)
            {
                EditorUtility.SetDirty(provider.target);
            }
        }

        public virtual void OnTweenerUpdating(DoTweenBaseProvider provider)
        {
            if (isTweenerTargetNeedSetDirty)
            {
                EditorUtility.SetDirty(provider.target);
            }
        }

        public virtual void OnDisable() => provider.StopPreview();
        public virtual void OnEnable() => provider = target as DoTweenBaseProvider;

        #region Auxiliary drawing methods

        static List<string> fields;

        [InitializeOnLoadMethod]
        static void LoadFieldsNameByRef()
        {
            fields = typeof(DoTweenBaseProvider)
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.DeclaredOnly)
                .Select(v => v.Name)
                .ToList();
        }

        internal bool IsPropertyDrawRequired(SerializedProperty property)
        {
            return !fields.Contains(property.name);
        }

        internal static void HorizontalLine(float height = 1)
        {
            float c = EditorGUIUtility.isProSkin ? 0.5f : 0.4f;
            HorizontalLine(new Color(c, c, c), height);
        }

        internal static void HorizontalLine(Color color, float height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            EditorGUI.DrawRect(rect, color);
            EditorGUILayout.GetControlRect(false, height);
        }

        #endregion
    }
}