using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector.Editor;

namespace OSK
{
    [CustomEditor(typeof(DoTweenBaseProvider), true)]
    public class DoTweenBaseProviderEditor : OdinEditor
    {
        private DoTweenBaseProvider provider;
        private bool generalParametersFoldout = true;
        private GUIStyle buttonStyle;
        private GUIStyle foldoutStyle;

        private float _previewTime = 0;
        private bool _isPlaying = false;
        private bool _isMoveRight = true;

        public void OnEnable() => provider = (DoTweenBaseProvider)target;
        public void OnDisable() => provider.StopPreview();

        public override void OnInspectorGUI()
        {
             InitializeStyles();
         
             provider = (DoTweenBaseProvider)target;
             GUI.enabled = EditorApplication.isPlaying || !provider.IsPreviewing();
     
             serializedObject.Update();
             serializedObject.ApplyModifiedProperties();
             
             base.OnInspectorGUI();
             DrawPreviewControls();
        }

        private void InitializeStyles()
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(EditorStyles.miniButtonRight)
                {
                    fixedWidth = 100,
                    richText = true
                };
            }

            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(EditorStyles.foldoutHeader)
                {
                    fontStyle = FontStyle.Normal,
                    fontSize = 14,
                    onNormal = { textColor = Color.white }
                };
            }
        }

        private void ValidateProperties()
        {
            var loopcount = serializedObject.FindProperty("settings.loopcount");
            loopcount.intValue = Mathf.Max(loopcount.intValue, -1);

            var duration = serializedObject.FindProperty("settings.duration");
            duration.floatValue = Mathf.Max(duration.floatValue, 0f);
        }

        private void DrawGeneralParameters()
        {
            generalParametersFoldout =
                EditorGUILayout.Foldout(generalParametersFoldout, "General Parameters", foldoutStyle);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);

            if (generalParametersFoldout)
            {
                DrawHorizontalLine();
                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);
              
                while (iterator.NextVisible(false))
                {
                    if (iterator.name != "settings.loopType" || provider.settings.loopcount != 0 && provider.settings.loopcount != 1)
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                }
            }
        }

        private void DrawPreviewControls()
        {
            GUILayout.Space(10);
            GUILayout.Label("-------------------------------------------------------------------------------------");
            GUILayout.Label("Preview", EditorStyles.boldLabel);
            GUILayout.Space(5);

            if (provider.gameObject.activeInHierarchy && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GUILayout.BeginHorizontal();
                var stBtnPlay = new GUIStyle(EditorStyles.miniButtonLeft)
                {
                    fontSize = 15,
                    fixedWidth = 35,
                    fixedHeight = 20,
                    normal = new GUIStyleState()
                    {
                        //background = MakeColorTexture(new Color32(46, 139, 87, 255))
                    }
                };

                if (GUILayout.Button("▶", stBtnPlay))
                {
                    if (_previewTime != 0)
                        ResumePreview();
                    else
                        StartPreview();
                }

                GUILayout.Space(5);

                var stBtnStop = new GUIStyle(EditorStyles.miniButtonMid)
                {
                    fontSize = 20,
                    fixedWidth = 35,
                    fixedHeight = 20,
                    contentOffset = new Vector2(0, -1.5f),
                    normal = new GUIStyleState()
                    {
                        //background = MakeColorTexture(new Color32(178, 34, 34, 255))
                    }
                };


                GUI.enabled = _previewTime != 0;
                if (GUILayout.Button("■", stBtnStop))
                {
                    StopPreview();
                }

                GUI.enabled = true;


                // GUILayout.Space(5); 
                // PreviewAll();

                GUILayout.Space(10);
                EditorGUI.BeginChangeCheck();

                float newPreviewTime = EditorGUILayout.Slider(_previewTime, 0, provider.Duration() - 0.001f);
                if (EditorGUI.EndChangeCheck())
                {
                    if (_previewTime == 0)
                        ResumePreview();
                    _previewTime = newPreviewTime;
                    provider.Preview(_previewTime);
                    _isPlaying = false;
                    EditorApplication.update -= UpdatePreview;
                }

                //GUILayout.Label($"{_previewTime:F2}s", GUILayout.Width(50));
                GUILayout.EndHorizontal();
            }
            else
            {
                StopPreview();
            }
        }

        private Texture2D MakeColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private void ResumePreview()
        {
            _isPlaying = true;
            provider.StartPreview(OnTweenerStart, OnTweenerUpdating);
            EditorApplication.update += UpdatePreview;
        }

        public void PreviewAll()
        {
            var providers = provider.GetComponentsInChildren<DoTweenBaseProvider>(true);
            var providerActive = providers
                .Where(v => v.gameObject.activeInHierarchy && v.enabled)
                .ToArray();

            if (providerActive != null && providerActive.Length > 0)
            {
                var anyIsPreviewing = providerActive.Any(v => v.IsPreviewing());
                var label = anyIsPreviewing ? "■" : "▶▶";

                var stBtnPreviewAll = new GUIStyle(EditorStyles.miniButtonMid)
                {
                    fontSize = 15,
                    fixedWidth = 35,
                    fixedHeight = 20,
                    normal = new GUIStyleState()
                    {
                        //background = MakeColorTexture(new Color32(70, 130, 180, 255))
                    }
                };

                if (GUILayout.Button(label, stBtnPreviewAll))
                {
                    if (anyIsPreviewing)
                    {
                        foreach (var item in providerActive)
                        {
                            item.StopPreview();
                        }
                    }
                    else
                    {
                        foreach (var item in providerActive)
                        {
                            item.StopPreview();
                            item.StartPreview(() => OnTweenerStart(), () => OnTweenerUpdating());
                        }
                    }
                }
            }
        }

        private void StartPreview()
        {
            _previewTime = 0;
            _isPlaying = true;

            provider.StartPreview(OnTweenerStart, OnTweenerUpdating);
            EditorApplication.update += UpdatePreview;
        }

        private void StopPreview()
        {
            _isPlaying = false;
            _previewTime = 0;
            provider.StopPreview();
            EditorApplication.update -= UpdatePreview;
        }


        private void UpdatePreview()
        {
            if (_isPlaying)
            {
                if (provider.settings.loopcount == -1)
                {
                    switch (provider.settings.loopType)
                    {
                        case LoopType.Restart:
                            _previewTime += Time.deltaTime;
                            _previewTime %= provider.Duration();
                            break;
                        case LoopType.Yoyo:
                            if (_isMoveRight)
                            {
                                _previewTime += Time.deltaTime;
                                if (_previewTime >= provider.Duration())
                                    _isMoveRight = false;
                            }
                            else
                            {
                                _previewTime -= Time.deltaTime;
                                if (_previewTime <= 0)
                                    _isMoveRight = true;
                            }

                            break;
                        case LoopType.Incremental:
                            _previewTime += Time.deltaTime;
                            break;
                    }
                }
                else
                {
                    _previewTime += Time.deltaTime;
                }

                if (_previewTime >= provider.Duration() && provider.settings.loopcount != -1)
                {
                    StopPreview();
                }

                provider.Preview(_previewTime);
                Repaint();
            }
        }


        private void OnTweenerStart()
        {
            if (DotweenProviderSettings.VerifyTargetNeedSetDirty(provider.target))
            {
                EditorUtility.SetDirty(provider.target);
            }
        }

        private void OnTweenerUpdating()
        {
            if (DotweenProviderSettings.VerifyTargetNeedSetDirty(provider.target))
            {
                EditorUtility.SetDirty(provider.target);
            }
        }


        private static void DrawHorizontalLine(float height = 1)
        {
            float colorValue = EditorGUIUtility.isProSkin ? 0.5f : 0.4f;
            Color color = new Color(colorValue, colorValue, colorValue);

            Rect rect = EditorGUILayout.GetControlRect(false, height);
            EditorGUI.DrawRect(rect, color);
        }
    }
}