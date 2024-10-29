#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    [CustomEditor(typeof(SoundDataSO))]
    public class SoundDataEditor : Editor
    {
        private bool showTable = true;
        private Dictionary<SoundType, bool> soundTypeFoldouts = new Dictionary<SoundType, bool>();

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SoundDataSO soundDataSo = (SoundDataSO)target;
            EditorGUILayout.Space();

            showTable = EditorGUILayout.Foldout(showTable, "Show Sound Info Table");
            if (!showTable) return;

            EditorGUILayout.Space(50);

            foreach (SoundType type in System.Enum.GetValues(typeof(SoundType)))
            {
                if (!soundTypeFoldouts.ContainsKey(type))
                    soundTypeFoldouts[type] = true;

                EditorGUILayout.Space(20);
                soundTypeFoldouts[type] = EditorGUILayout.Foldout(soundTypeFoldouts[type], type.ToString() + " Sounds");

                if (soundTypeFoldouts[type])
                {
                    DrawTableHeaders();
                    EditorGUILayout.Space();

                    for (int i = 0; i < soundDataSo.ListSoundInfos.Count; i++)
                    {
                        SoundInfo soundInfo = soundDataSo.ListSoundInfos[i];

                        if (soundInfo.type == type)
                        {
                            DrawRowBorder();
                            EditorGUILayout.BeginHorizontal();

                            AudioClip newAudioClip = (AudioClip)EditorGUILayout.ObjectField(soundInfo.audioClip, typeof(AudioClip), false, GUILayout.Width(150));
                            if (newAudioClip != soundInfo.audioClip)
                            {
                                if (IsDuplicateAudioClip(soundDataSo, newAudioClip))
                                {
                                    Logg.LogError($"AudioClip {newAudioClip.name} already exists in the list. Cannot add duplicate.");
                                }
                                else
                                {
                                    soundInfo.audioClip = newAudioClip;
                                    soundInfo.UpdateId();
                                }
                            }

                            soundInfo.type = (SoundType)EditorGUILayout.EnumPopup(soundInfo.type, GUILayout.Width(75));

                            GUI.enabled = soundInfo.audioClip != null && !soundInfo.IsPlaying();
                            if (GUILayout.Button("Play", GUILayout.Width(50)))
                            {
                                soundInfo.Play();
                            }

                            GUI.enabled = soundInfo.audioClip != null && soundInfo.IsPlaying();
                            if (GUILayout.Button("Stop", GUILayout.Width(50)))
                            {
                                soundInfo.Stop();
                            }

                            GUI.enabled = true;
                            if (GUILayout.Button("Remove", GUILayout.Width(60)))
                            {
                                soundDataSo.ListSoundInfos.RemoveAt(i);
                                i--;
                                continue;
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    DrawRowBorder();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.Space(50);
            if (GUILayout.Button("Add New Sound Info", GUILayout.Width(200)))
            {
                soundDataSo.ListSoundInfos.Add(new SoundInfo());
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawRowBorder()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, Color.gray);
        }

        private void DrawTableHeaders()
        {
            EditorGUILayout.BeginHorizontal();
            DrawTableCell("Audio Clip", 150, true);
            EditorGUILayout.LabelField("Type", GUILayout.Width(75));
            EditorGUILayout.LabelField("Actions", GUILayout.Width(150));
            EditorGUILayout.EndHorizontal();
            DrawRowBorder();
        }

        private void DrawTableCell(string text, float width, bool isHeader = false)
        {
            if (isHeader)
            {
                EditorGUILayout.LabelField(text, EditorStyles.boldLabel, GUILayout.Width(width));
            }
            else
            {
                EditorGUILayout.LabelField(text, GUILayout.Width(width));
            }
        }

        private bool IsDuplicateAudioClip(SoundDataSO soundDataSo, AudioClip clipToCheck)
        {
            if (clipToCheck == null) return false;

            foreach (var soundInfo in soundDataSo.ListSoundInfos)
            {
                if (soundInfo.audioClip == clipToCheck)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif
