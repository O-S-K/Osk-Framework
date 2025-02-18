#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace OSK
{
    [CustomEditor(typeof(ListSoundSO))]
    public class SoundDataEditor : Editor
    {
        private bool showTable = true;
        private Dictionary<SoundType, bool> soundTypeFoldouts = new Dictionary<SoundType, bool>();

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ListSoundSO listSoundSo = (ListSoundSO)target;
            EditorGUILayout.Space();
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("----------------------------------------------------------------------------------------------");

            showTable = EditorGUILayout.Foldout(showTable, "Show Sound Info Table");
            if (!showTable) return;

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

                    for (int i = 0; i < listSoundSo.ListSoundInfos.Count; i++)
                    {
                        SoundData soundData = listSoundSo.ListSoundInfos[i];

                        if (soundData.type == type)
                        {
                            DrawRowBorder();
                            EditorGUILayout.BeginHorizontal();

                            AudioClip newAudioClip = (AudioClip)EditorGUILayout.ObjectField(soundData.audioClip, typeof(AudioClip), false, GUILayout.Width(150));
                            if (newAudioClip != soundData.audioClip)
                            {
                                if (IsDuplicateAudioClip(listSoundSo, newAudioClip))
                                {
                                    Logg.LogError($"AudioClip {newAudioClip.name} already exists in the list. Cannot add duplicate.");
                                }
                                else
                                {
                                    soundData.audioClip = newAudioClip;
                                    soundData.UpdateId();
                                }
                            }

                            soundData.type = (SoundType)EditorGUILayout.EnumPopup(soundData.type, GUILayout.Width(75));

                            GUI.enabled = soundData.audioClip != null && !soundData.IsPlaying();
                            if (GUILayout.Button("Play", GUILayout.Width(50)))
                            {
                                soundData.Play();
                            }

                            GUI.enabled = soundData.audioClip != null && soundData.IsPlaying();
                            if (GUILayout.Button("Stop", GUILayout.Width(50)))
                            {
                                soundData.Stop();
                            }

                            GUI.enabled = true;
                            if (GUILayout.Button("Remove", GUILayout.Width(60)))
                            {
                                listSoundSo.ListSoundInfos.RemoveAt(i);
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
                listSoundSo.ListSoundInfos.Add(new SoundData());
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

        private bool IsDuplicateAudioClip(ListSoundSO listSoundSo, AudioClip clipToCheck)
        {
            if (clipToCheck == null) return false;

            foreach (var soundInfo in listSoundSo.ListSoundInfos)
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
