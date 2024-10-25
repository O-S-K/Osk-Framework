#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OSK
{
    [CustomEditor(typeof(SoundData))]
    public class SoundDataEditor : Editor
    {
        private bool showTable = true;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SoundData soundData = (SoundData)target;
            EditorGUILayout.Space();

            showTable = EditorGUILayout.Foldout(showTable, "Show Sound Info Table");
            if(!showTable)
                return;
            
            // Draw table headers
            DrawTableHeaders();

            EditorGUILayout.Space();

            // Loop through each SoundInfo in the list
            for (int i = 0; i < soundData.ListSoundInfos.Count; i++)
            {
                SoundInfo soundInfo = soundData.ListSoundInfos[i];

                // Draw horizontal line for each row
                DrawRowBorder();

                EditorGUILayout.BeginHorizontal();

                // Drag and drop for AudioClip, with duplicate check
                AudioClip newAudioClip = (AudioClip)EditorGUILayout.ObjectField(soundInfo.audioClip, typeof(AudioClip), false, GUILayout.Width(150));

                // If AudioClip changed, check for duplicates
                if (newAudioClip != soundInfo.audioClip)
                {
                    if (IsDuplicateAudioClip(soundData, newAudioClip))
                    {
                        Logg.LogError($"AudioClip {newAudioClip.name} already exists in the list. Cannot add duplicate.");
                    }
                    else
                    {
                        soundInfo.audioClip = newAudioClip;
                        soundInfo.UpdateId(); // Update the ID based on the new AudioClip name
                    }
                }
                
                // Display the ID
                //DrawTableCell(soundInfo.id, 100);

                // Display Enum dropdown for AudioType
                soundInfo.type = (SoundType)EditorGUILayout.EnumPopup(soundInfo.type, GUILayout.Width(75));

                // Play button
                GUI.enabled = soundInfo.audioClip != null && !soundInfo.IsPlaying();
                if (GUILayout.Button("Play", GUILayout.Width(50)))
                {
                    soundInfo.Play();
                }

                // Stop button
                GUI.enabled = soundInfo.audioClip != null && soundInfo.IsPlaying();
                if (GUILayout.Button("Stop", GUILayout.Width(50)))
                {
                    soundInfo.Stop();
                }

                // Re-enable GUI for the Remove button
                GUI.enabled = true;
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    soundData.ListSoundInfos.RemoveAt(i);
                    i--;
                    continue;
                }

                EditorGUILayout.EndHorizontal();
            }

            // Draw bottom border
            DrawRowBorder();

            // Add button to create new SoundInfo
            EditorGUILayout.Space(); // Add space before the button
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(); // Center the button
            if (GUILayout.Button("Add New Sound Info", GUILayout.Width(200)))
            {
                soundData.ListSoundInfos.Add(new SoundInfo()); // Add new SoundInfo element
            }
            EditorGUILayout.Space(); // Add space on the right
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        // Draw border between rows
        private void DrawRowBorder()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, Color.gray);
        }

        // Draw table headers
        private void DrawTableHeaders()
        {
            EditorGUILayout.BeginHorizontal();

            // Headers with equal padding
            DrawTableCell("Audio Clip", 150, true);
            //DrawTableCell("ID", 100, true);
            EditorGUILayout.LabelField("Type", GUILayout.Width(75));
            EditorGUILayout.LabelField("Actions", GUILayout.Width(150));

            EditorGUILayout.EndHorizontal();

            // Draw border under headers
            DrawRowBorder();
        }

        // Draw table cell with specified width
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

        // Check for duplicate AudioClip in the list
        private bool IsDuplicateAudioClip(SoundData soundData, AudioClip clipToCheck)
        {
            if (clipToCheck == null) return false; // If no clip selected, it's not a duplicate

            foreach (var soundInfo in soundData.ListSoundInfos)
            {
                if (soundInfo.audioClip == clipToCheck)
                {
                    return true; // Duplicate found
                }
            }
            return false; // No duplicates found
        }
    }
}
#endif
