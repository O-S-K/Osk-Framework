using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using GameUtil;
using Sirenix.OdinInspector.Editor;
using UnityEngine.UI;

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
            provider = (DoTweenBaseProvider)target;
            GUI.enabled = EditorApplication.isPlaying || !provider.IsPreviewing();

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            InitializeStyles();
            DrawGeneralParameters();
            base.OnInspectorGUI();
            DrawStartEndButtons();
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

        private void DrawGeneralParameters()
        {
            generalParametersFoldout =
                EditorGUILayout.Foldout(generalParametersFoldout, "General Parameters", foldoutStyle);

            if (generalParametersFoldout)
            {
                DrawFieldObject();
            }
        }

        private bool showRoot;
        private bool showImage;
        private bool showCanvasGroup;
        private bool showGraphic;
        private bool showText;

        private void DrawFieldObject()
        {
            #region Root

            showRoot = provider is not CanvasGroupProvider
                       && provider is not FillAmountProvider
                       && provider is not GraphicProvider
                       && provider is not TMPNumScrollProvider
                       && provider is not TextLoadingAnimationProvider
                       && provider is not GameObjectProvider
                       && provider is not EventProvider
                       && provider is not EffectProvider;
            
            showImage = provider.GetComponent<Image>() != null
                        && provider.GetComponent<FillAmountProvider>() != null;

            showCanvasGroup = provider.GetComponent<CanvasGroup>() != null
                              && provider.GetComponent<CanvasGroupProvider>() != null;

            showGraphic = provider.GetComponent<Graphic>() != null
                          && provider.GetComponent<GraphicProvider>() != null;

            showText = provider.GetComponent<Text>() != null
                       && (provider.GetComponent<TMPNumScrollProvider>() == provider
                           || provider.GetComponent<TextLoadingAnimationProvider>() == provider);

            
            if (showRoot)
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                SerializedProperty rootValueProperty = serializedObject.FindProperty("settings.root");

                if (GUILayout.Button("Set Root", GUILayout.Width(100)))
                {
                    switch (rootValueProperty.propertyType)
                    {
                        case SerializedPropertyType.ObjectReference:

                            if (provider.GetComponent<Transform>() != null)
                            {
                                rootValueProperty.objectReferenceValue = provider.transform;
                            }
                            else if (provider.GetComponent<RectTransform>() != null)
                            {
                                rootValueProperty.objectReferenceValue = provider.transform.GetRectTransform();
                            }
                            break;
                    }
                }

                if (GUILayout.Button("Delete", GUILayout.Width(50)))
                {
                    rootValueProperty.objectReferenceValue = null;
                }

                EditorGUILayout.PropertyField(rootValueProperty, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
            }

            #endregion
            #region Image
            else if (showImage)
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                SerializedProperty imageValueProperty = serializedObject.FindProperty("image");
                if (GUILayout.Button("Set Image", GUILayout.Width(100)))
                {
                    switch (imageValueProperty.propertyType)
                    {
                        case SerializedPropertyType.ObjectReference:
                            imageValueProperty.objectReferenceValue = provider.GetComponent<Image>();
                            break;
                    }
                }

                if (GUILayout.Button("Delete", GUILayout.Width(50)))
                {
                    imageValueProperty.objectReferenceValue = null;
                }

                EditorGUILayout.PropertyField(imageValueProperty, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region CanvasGroup
            else if (showCanvasGroup)
            {
                if (provider is CanvasGroupProvider)
                {
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty canvasGroupValueProperty = serializedObject.FindProperty("canvasGroup");
                    if (GUILayout.Button("Canvas Group", GUILayout.Width(100)))
                    {
                        switch (canvasGroupValueProperty.propertyType)
                        {
                            case SerializedPropertyType.ObjectReference:
                                canvasGroupValueProperty.objectReferenceValue = provider.GetComponent<CanvasGroup>();
                                break;
                        }
                    }

                    if (GUILayout.Button("Delete", GUILayout.Width(50)))
                    {
                        canvasGroupValueProperty.objectReferenceValue = null;
                    }

                    EditorGUILayout.PropertyField(canvasGroupValueProperty, new GUIContent(""));
                    EditorGUILayout.EndHorizontal();
                }
            }

            #endregion

            #region Graphic
            else if (showGraphic)
            {
                if (provider is GraphicProvider)
                {
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty graphicValueProperty = serializedObject.FindProperty("graphic");
                    if (GUILayout.Button("Set Graphic", GUILayout.Width(100)))
                    {
                        switch (graphicValueProperty.propertyType)
                        {
                            case SerializedPropertyType.ObjectReference:
                                graphicValueProperty.objectReferenceValue = provider.GetComponent<Graphic>();
                                break;
                        }
                    }

                    if (GUILayout.Button("Delete", GUILayout.Width(50)))
                    {
                        graphicValueProperty.objectReferenceValue = null;
                    }

                    EditorGUILayout.PropertyField(graphicValueProperty, new GUIContent(""));
                    EditorGUILayout.EndHorizontal();
                }
            }

            #endregion

            #region Text
            else if (showText)
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                SerializedProperty textValueProperty = serializedObject.FindProperty("text");
                if (GUILayout.Button("Set Text", GUILayout.Width(100)))
                {
                    switch (textValueProperty.propertyType)
                    {
                        case SerializedPropertyType.ObjectReference:
                            textValueProperty.objectReferenceValue = provider.GetComponent<Text>();
                            break;
                    }
                }

                if (GUILayout.Button("Delete", GUILayout.Width(50)))
                {
                    textValueProperty.objectReferenceValue = null;
                }

                EditorGUILayout.PropertyField(textValueProperty, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
            }

            #endregion
            
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target); 
        }

        private void DrawSettingTween()
        {
            #region MyRegion

            EditorGUILayout.LabelField("Setting", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.playOnEnable"),
                new GUIContent("Play On Enable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.setAutoKill"),
                new GUIContent("Set Auto Kill"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.delay"), new GUIContent("Delay"));

            EditorGUILayout.Slider(serializedObject.FindProperty("settings.duration"), 0, 100,
                new GUIContent("Duration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.loopcount"),
                new GUIContent("Loop Count"));

            var loopcount = serializedObject.FindProperty("settings.loopcount");
            loopcount.intValue = Mathf.Max(loopcount.intValue, -1);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.loopType"),
                new GUIContent("Loop Type"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.typeAnim"),
                new GUIContent("Type Animation"));

            if (provider.settings.typeAnim == TypeAnimation.Ease)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.ease"), new GUIContent("Ease"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.curve"), new GUIContent("Curve"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.updateType"),
                new GUIContent("Update Type"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.useUnscaledTime"),
                new GUIContent("Use Unscaled Time"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings.eventCompleted"),
                new GUIContent("Event Completed"));

            if (showRoot)
            {
                var isLocal = serializedObject.FindProperty("isLocal");
                var isResetToFrom = serializedObject.FindProperty("isResetToFrom");

                if (isLocal != null)
                    EditorGUILayout.PropertyField(isLocal, new GUIContent("Set Local"));
                if (isResetToFrom != null)
                    EditorGUILayout.PropertyField(isResetToFrom, new GUIContent("Reset To From"));

                if (provider is MoveAlongPathProvider)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("paths"), new GUIContent("Paths"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("typePath"),
                        new GUIContent("Type Path"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("pathMode"),
                        new GUIContent("Path Mode"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isClosedPath"),
                        new GUIContent("Is Closed Path"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isLocal"), new GUIContent("Is Local"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isStartFirstPoint"),
                        new GUIContent("Is Start First Point"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isLookAt"),
                        new GUIContent("Is Look At"));
                }
                else if (provider is RotationProvider)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateMode"),
                        new GUIContent("Rotate Mode"));
                }
                else if (provider is ShakeProvider)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("typeShake"),
                        new GUIContent("Type Shake"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isRandom"),
                        new GUIContent("Is Random"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("strength"),
                        new GUIContent("Strength"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("vibrato"), new GUIContent("Vibrato"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("randomness"),
                        new GUIContent("Randomness"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("snapping"),
                        new GUIContent("Snapping"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeOut"), new GUIContent("Fade Out"));
                }
                else if (provider is PunchProvider)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("typeShake"),
                        new GUIContent("Type Shake"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isRandom"),
                        new GUIContent("Is Random"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("strength"),
                        new GUIContent("Strength"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("vibrato"), new GUIContent("Vibrato"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("elasticity"),
                        new GUIContent("Elasticity"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("snapping"),
                        new GUIContent("Snapping"));
                }
            }
            else if (provider is EffectProvider)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("particleSystem"),
                    new GUIContent("Vfx Effect"));
            }
            else if (provider is EventProvider)
            {
                // EditorGUILayout.PropertyField(serializedObject.FindProperty("onComplete"), new GUIContent("Event Trigger"));
            }
            else if (provider is GameObjectProvider)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gameObject"),
                    new GUIContent("Game Object"));
            }

            #endregion Target
        }

        private void DrawStartEndButtons()
        {
            if (provider is EventProvider ||
                provider is EffectProvider ||
                provider is MoveAlongPathProvider ||
                provider is PunchProvider ||
                provider is ShakeProvider)

                return;

            SerializedProperty startValueProperty = serializedObject.FindProperty("from");
            SerializedProperty endValueProperty = serializedObject.FindProperty("to");

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set From", GUILayout.Width(100)))
            {
                switch (startValueProperty.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        startValueProperty.intValue = (int)provider.GetStartValue();
                        break;
                    case SerializedPropertyType.Boolean:
                        EditorGUILayout.LabelField("Is Active");
                        startValueProperty.boolValue = (bool)provider.GetStartValue();
                        break;
                    case SerializedPropertyType.Float:
                        if (provider is FillAmountProvider)
                        {
                            startValueProperty.floatValue = (float)provider.GetComponent<Image>().fillAmount;
                        }
                        else
                        {
                            startValueProperty.floatValue = (float)provider.GetStartValue();
                        }

                        break;
                    case SerializedPropertyType.Color:
                        var graphic = provider.GetComponent<Graphic>();
                        if (graphic != null)
                        {
                            startValueProperty.colorValue = graphic.color;
                        }
                        else
                        {
                            startValueProperty.colorValue = (Color)provider.GetStartValue();
                        }

                        break;
                    case SerializedPropertyType.String:
                        startValueProperty.stringValue = (string)provider.GetStartValue();
                        break;
                    case SerializedPropertyType.Enum:
                        startValueProperty.enumValueIndex = (int)provider.GetStartValue();
                        break;
                    case SerializedPropertyType.Vector3:
                        var p = provider.GetComponent<Transform>();
                        if (provider is RotationProvider)
                        {
                            var r = provider.GetComponent<RotationProvider>();
                            startValueProperty.vector3Value = r.isLocal
                                ? p.localEulerAngles
                                : p.eulerAngles;
                        }
                        else if (provider is ScaleProvider)
                        {
                            startValueProperty.vector3Value = p.localScale;
                        }
                        else
                        {
                            if (p != null)
                            {
                                startValueProperty.vector3Value = p.localPosition;
                            }
                            else
                            {
                                startValueProperty.vector3Value = (Vector3)provider.GetStartValue();
                            }
                        }

                        break;
                    case SerializedPropertyType.Quaternion:
                        var q = provider.GetComponent<Transform>();
                        if (q != null)
                        {
                            startValueProperty.vector3Value = q.localRotation.eulerAngles;
                        }
                        else
                        {
                            startValueProperty.vector3Value = (Vector3)provider.GetStartValue();
                        }

                        break;
                }
            }

            EditorGUILayout.PropertyField(startValueProperty, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set To", GUILayout.Width(100)))
            {
                switch (endValueProperty.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        endValueProperty.intValue = (int)provider.GetEndValue();
                        break;
                    case SerializedPropertyType.Boolean:
                        EditorGUILayout.LabelField("Is Active");
                        endValueProperty.boolValue = (bool)provider.GetEndValue();
                        break;
                    case SerializedPropertyType.Float:
                        if (provider is FillAmountProvider)
                        {
                            endValueProperty.floatValue = (float)provider.GetComponent<Image>().fillAmount;
                        }
                        else
                        {
                            endValueProperty.floatValue = (float)provider.GetEndValue();
                        }

                        break;
                    case SerializedPropertyType.Color:
                        var graphic = provider.GetComponent<Graphic>();
                        if (graphic != null)
                        {
                            endValueProperty.colorValue = graphic.color;
                        }
                        else
                        {
                            endValueProperty.colorValue = (Color)provider.GetEndValue();
                        }

                        break;
                    case SerializedPropertyType.String:
                        endValueProperty.stringValue = (string)provider.GetEndValue();
                        break;
                    case SerializedPropertyType.Enum:
                        endValueProperty.enumValueIndex = (int)provider.GetEndValue();
                        break;
                    case SerializedPropertyType.Vector3:
                        var p = provider.GetComponent<Transform>();
                        if (provider is RotationProvider)
                        {
                            var r = provider.GetComponent<RotationProvider>();
                            endValueProperty.vector3Value = r.isLocal
                                ? p.localEulerAngles
                                : p.eulerAngles;
                        }
                        else if (provider is ScaleProvider)
                        {
                            endValueProperty.vector3Value = p.localScale;
                        }
                        else
                        {
                            if (p != null)
                            {
                                endValueProperty.vector3Value = p.localPosition;
                            }
                            else
                            {
                                endValueProperty.vector3Value = (Vector3)provider.GetEndValue();
                            }
                        }

                        break;
                    case SerializedPropertyType.Quaternion:
                        var q = provider.GetComponent<Transform>();
                        if (q != null)
                        {
                            endValueProperty.vector3Value = q.localRotation.eulerAngles;
                        }
                        else
                        {
                            endValueProperty.vector3Value = (Vector3)provider.GetEndValue();
                        }

                        break;
                }
            }

            EditorGUILayout.PropertyField(endValueProperty, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
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


                GUILayout.Space(5);
                PreviewAll();

                GUILayout.Space(10);
                EditorGUI.BeginChangeCheck();

                float newPreviewTime = EditorGUILayout.Slider(_previewTime, 0, provider.GetDuration() - 0.001f);
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
                bool showButton = providers.Length > 1;

                if (showButton && GUILayout.Button(label, stBtnPreviewAll))
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
                            _previewTime %= provider.GetDuration();
                            break;
                        case LoopType.Yoyo:
                            if (_isMoveRight)
                            {
                                _previewTime += Time.deltaTime;
                                if (_previewTime >= provider.GetDuration())
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

                if (_previewTime >= provider.GetDuration() && provider.settings.loopcount != -1)
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