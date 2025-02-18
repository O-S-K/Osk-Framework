using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(UIParticleSO))]
    public class UIParticleSOEditor : OdinEditor
    {
        private SerializedProperty _effectSettingsProperty;

        private void OnEnable()
        {
            _effectSettingsProperty = serializedObject.FindProperty("_effectSettings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle titleStyle0 = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = Color.yellow }
            };

            EditorGUILayout.LabelField("--- Effect Settings ---", titleStyle0);
            GUILayout.Space(10);

            for (int i = 0; i < _effectSettingsProperty.arraySize; i++)
            {
                var element = _effectSettingsProperty.GetArrayElementAtIndex(i);
                string settingName = element.FindPropertyRelative("name").stringValue;

                if (string.IsNullOrEmpty(settingName))
                {
                    settingName = $"Effect Setting {i + 1}";
                }

                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, settingName, true);

                if (element.isExpanded)
                {
                    EditorGUILayout.BeginVertical("box"); 
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("name"));
                  //  EditorGUILayout.PropertyField(element.FindPropertyRelative("icon"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("numberOfEffects"));
                    EditorGUILayout.PropertyField(element.FindPropertyRelative("isDrop"));
                    EditorGUI.indentLevel--;

                    GUILayout.Space(5);

                    // Drop Section
                    if (element.FindPropertyRelative("isDrop").boolValue)
                    {
                        GUIStyle titleStyle1 = new GUIStyle(EditorStyles.boldLabel)
                        {
                            normal = { textColor = Color.green }
                        };

                        EditorGUILayout.LabelField("Drop Settings", titleStyle1);
                        EditorGUILayout.BeginVertical("box"); 
                        DrawColoredBox(() =>
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("sphereRadius"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("delayDrop"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("timeDrop"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("typeAnimationDrop"));
                            var typeAnimationDrop = (TypeAnimation)element.FindPropertyRelative("typeAnimationDrop").enumValueIndex;
                            if (typeAnimationDrop == TypeAnimation.Ease)
                            {
                                EditorGUILayout.PropertyField(element.FindPropertyRelative("easeDrop"));
                            }
                            else if (typeAnimationDrop == TypeAnimation.Curve)
                            {
                                EditorGUILayout.PropertyField(element.FindPropertyRelative("curveDrop"));
                            }
                            
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("isScaleDrop"));
                            if (element.FindPropertyRelative("isScaleDrop").boolValue)
                            {
                                EditorGUILayout.PropertyField(element.FindPropertyRelative("scaleDropStart"));
                                EditorGUILayout.PropertyField(element.FindPropertyRelative("scaleDropEnd"));
                            }  
                            
                        }, Color.green);
                        EditorGUILayout.EndVertical(); 
                    }

                    GUILayout.Space(10);


                    GUIStyle titleStyle2 = new GUIStyle(EditorStyles.boldLabel)
                    {
                        normal = { textColor = Color.cyan }
                    };

                    // Move Section
                    EditorGUILayout.LabelField("Move Settings", titleStyle2);
                    EditorGUILayout.BeginVertical("box"); 
                    DrawColoredBox(() =>
                    {
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("typeMove"));
                        var typeMove = (TypeMove)element.FindPropertyRelative("typeMove").enumValueIndex;
                        if (typeMove is TypeMove.Path or TypeMove.Beziers or TypeMove.CatmullRom)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("paths"));
                        }
                        else if (typeMove == TypeMove.Straight)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("height"));
                        }
                        else if (typeMove == TypeMove.DoJump)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("jumpPower"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("jumpNumber"));
                        }

                        else if (typeMove == TypeMove.Around)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("midPointOffsetX"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("midPointOffsetZ"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("height"));
                        }
                        else if (typeMove == TypeMove.Sin)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("pointsCount"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("height"));
                        }


                        EditorGUILayout.PropertyField(element.FindPropertyRelative("typeAnimationMove"));
                        var typeAnimationMove =
                            (TypeAnimation)element.FindPropertyRelative("typeAnimationMove").enumValueIndex;
                        if (typeAnimationMove == TypeAnimation.Ease)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("easeMove"));
                        }
                        else if (typeAnimationMove == TypeAnimation.Curve)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("curveMove"));
                        }

                        EditorGUILayout.PropertyField(element.FindPropertyRelative("timeMove"));
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("delayMove"));
                        
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("isScaleMove"));
                        if (element.FindPropertyRelative("isScaleMove").boolValue)
                        {
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("scaleMoveStart"));
                            EditorGUILayout.PropertyField(element.FindPropertyRelative("scaleMoveTarget"));
                        } 
                        
                    }, Color.cyan);
                    EditorGUILayout.EndVertical(); 

                    GUILayout.Space(10);

                    if (GUILayout.Button("Remove"))
                    {
                        _effectSettingsProperty.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    EditorGUILayout.EndVertical(); 
                }
            }

            GUILayout.Space(30);
            if (GUILayout.Button("Add Effect Setting"))
            {
                _effectSettingsProperty.InsertArrayElementAtIndex(_effectSettingsProperty.arraySize);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawColoredBox(System.Action drawContent, Color color)
        {
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            EditorGUILayout.BeginVertical("box");
            drawContent();
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = originalColor;
        }
    }
}
#endif