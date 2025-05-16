#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace OSK
{
    [CustomEditor(typeof(ListSoundSO))]
    public class SoundDataEditor : Editor
    {
        private Dictionary<string, Dictionary<SoundType, bool>> soundTypeFoldoutsPerGroup =
            new Dictionary<string, Dictionary<SoundType, bool>>();

        private Dictionary<string, bool> groupFoldouts = new Dictionary<string, bool>();

        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = true)]
        private static List<string> groupNames = new List<string>() { "Music", "UI" };

        private ListSoundSO listSoundSo;

        private bool showTable = true;
        private bool showGroupNames = true;

        public override void OnInspectorGUI()
        {
            listSoundSo = (ListSoundSO)target;
            DrawDefaultInspector();

            if (GUILayout.Button("Sort To Type"))
            {
                SortToType(listSoundSo);
            }

            if (GUILayout.Button("Set ID For Name Clip"))
            {
                SetIDForNameClip();
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField(
                "----------------------------------------------------------------------------------------------");

            showTable = EditorGUILayout.Foldout(showTable, "Show Sound Info Table");
            if (!showTable) return;

            GUILayout.Space(10);
            showGroupNames = EditorGUILayout.Foldout(showGroupNames, "Show Group Names");
            if (showGroupNames)
            {
                DrawGroupNames();
            }

            if (groupNames == null || groupNames.Count == 0)
            {
                groupNames?.Add("Default");
            }

            var groups = listSoundSo.ListSoundInfos
                .Select(x => string.IsNullOrEmpty(x.group) ? "Default" : x.group)
                .Distinct()
                .OrderBy(x => x);


            EditorGUILayout.Space(20);
            EditorGUILayout.TextArea("Sound Groups", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            foreach (var group in groups)
            {
                if (!groupFoldouts.ContainsKey(group))
                    groupFoldouts[group] = true;

                if (!soundTypeFoldoutsPerGroup.ContainsKey(group))
                    soundTypeFoldoutsPerGroup[group] = new Dictionary<SoundType, bool>();

                EditorGUI.indentLevel += 1;
                groupFoldouts[group] = EditorGUILayout.Foldout(groupFoldouts[group], $"Group: {group}");
                EditorGUI.indentLevel -= 1;


                if (!groupFoldouts[group]) continue;

                DrawTableHeaders();

                foreach (SoundType type in System.Enum.GetValues(typeof(SoundType)))
                {
                    // Kiểm tra có SoundType này trong group không
                    if (!listSoundSo.ListSoundInfos.Any(x => x.type == type && x.group == group))
                        continue;

                    if (!soundTypeFoldoutsPerGroup[group].ContainsKey(type))
                        soundTypeFoldoutsPerGroup[group][type] = true;

                    EditorGUI.indentLevel += 2;
                    soundTypeFoldoutsPerGroup[group][type] =
                        EditorGUILayout.Foldout(soundTypeFoldoutsPerGroup[group][type], type.ToString());
                    EditorGUI.indentLevel -= 2;

                    if (!soundTypeFoldoutsPerGroup[group][type]) continue;

                    for (int i = 0; i < listSoundSo.ListSoundInfos.Count; i++)
                    {
                        SoundData soundData = listSoundSo.ListSoundInfos[i];

                        if (soundData.type != type || soundData.group != group)
                            continue;

                        EditorGUI.indentLevel += 2;
                        DrawRowBorder();
                        EditorGUILayout.BeginHorizontal();

                        // AudioClip
                        AudioClip newAudioClip = (AudioClip)EditorGUILayout.ObjectField(soundData.audioClip,
                            typeof(AudioClip), false, GUILayout.Width(150));
                        if (newAudioClip != soundData.audioClip)
                        {
                            if (IsDuplicateAudioClip(listSoundSo, newAudioClip))
                            {
                                Logg.LogError(
                                    $"AudioClip {newAudioClip.name} already exists in the list. Cannot add duplicate.");
                            }
                            else
                            {
                                soundData.audioClip = newAudioClip;
                            }
                        }

                        soundData.UpdateId();
                        int currentGroupIndex = groupNames.IndexOf(soundData.group);
                        if (currentGroupIndex == -1)
                        {
                            currentGroupIndex = 0;
                            soundData.group = groupNames[0];
                        }

                        // Group Dropdown
                        int newGroupIndex = EditorGUILayout.Popup(currentGroupIndex, groupNames.ToArray(),
                            GUILayout.Width(100));
                        if (newGroupIndex != currentGroupIndex)
                        {
                            soundData.group = groupNames[newGroupIndex];
                        }

                        // Type Dropdown
                        GUILayout.Space(-30);
                        soundData.type = (SoundType)EditorGUILayout.EnumPopup(soundData.type, GUILayout.Width(110));

                        // Volume Slider
                        GUILayout.Label(EditorGUIUtility.IconContent("d_AudioSource Icon"), GUILayout.Width(20),
                            GUILayout.Height(20));
                        float newVolume = GUILayout.HorizontalSlider(soundData.volume, 0f, 1f, GUILayout.Width(50));

                        if (Mathf.Abs(newVolume - soundData.volume) > 0.01f)
                        {
                            soundData.volume = newVolume;
                            soundData.SetVolume(newVolume);
                        }

                        GUILayout.Label(soundData.volume.ToString("F1"), GUILayout.Width(25));

                   
                        float oldMin = soundData.pitch.min;
                        float oldMax = soundData.pitch.max;

                        GUILayout.Space(-40);
                        // Pitch Slider
                        Rect sliderRect = GUILayoutUtility.GetRect(100, 20, GUILayout.ExpandWidth(false));
                        float newMin = oldMin;
                        float newMax = oldMax;
                        
                        EditorGUI.MinMaxSlider(sliderRect, ref newMin, ref newMax, 0.1f, 2.0f);

                        string minStr = newMin.ToString("F1");
                        string maxStr = newMax.ToString("F1");

                        GUILayout.Space(-40);
                        minStr = EditorGUILayout.DelayedTextField(minStr, GUILayout.Width(70));
                        GUILayout.Space(-40);
                        maxStr = EditorGUILayout.DelayedTextField(maxStr, GUILayout.Width(70));

                        if (float.TryParse(minStr, out float parsedMin))
                        {
                            newMin = Mathf.Clamp(Mathf.Round(parsedMin * 10f) / 10f, 0.1f, newMax);
                        }

                        if (float.TryParse(maxStr, out float parsedMax))
                        {
                            newMax = Mathf.Clamp(Mathf.Round(parsedMax * 10f) / 10f, newMin, 2.0f);
                        }

                        if (Mathf.Abs(newMin - oldMin) > 0.01f || Mathf.Abs(newMax - oldMax) > 0.01f)
                        {
                            var newPitch = new MinMaxFloat(newMin, newMax); 
                            soundData.pitch = newPitch;                     
                            soundData.SetPitch(newPitch); 
                        }

                        // Play Button
                        GUILayout.Space(30);
                        GUI.enabled = soundData.audioClip != null && !soundData.IsPlaying();
                        GUI.color = soundData.IsPlaying() ? Color.green : Color.white;

                        if (GUILayout.Button("Play", GUILayout.Width(50)))
                            soundData.Play(soundData.pitch);

                        GUI.color = Color.white;

                        // Stop Button
                        GUI.enabled = soundData.audioClip != null && soundData.IsPlaying();
                        if (GUILayout.Button("Stop", GUILayout.Width(50)))
                            soundData.Stop();

                        // Remove
                        GUI.enabled = true;
                        if (GUILayout.Button("Remove", GUILayout.Width(60)))
                        {
                            listSoundSo.ListSoundInfos.RemoveAt(i);
                            i--;
                            continue;
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel -= 2;
                    }

                    DrawRowBorder();
                }
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space(50);
            if (GUILayout.Button("Add New Sound Info", GUILayout.Width(200)))
            {
                listSoundSo.ListSoundInfos.Add(new SoundData());
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Select Folder Path", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Folder Path", listSoundSo.filePathSoundID);

            if (GUILayout.Button("Select Folder", GUILayout.Width(100)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Folder", listSoundSo.filePathSoundID, "");
                if (!string.IsNullOrEmpty(path))
                {
                    path = "Assets" + path.Replace(Application.dataPath, "");
                    listSoundSo.filePathSoundID = path + "/SoundID.cs";
                    EditorUtility.SetDirty(listSoundSo);
                }
            }

            if (GUILayout.Button("Open Folder", GUILayout.Width(100)))
            {
                string path = listSoundSo.filePathSoundID;
                path = path.Replace("/SoundID.cs", "");
                EditorUtility.RevealInFinder(path);
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Enum ID"))
            {
                var listSound = listSoundSo.ListSoundInfos
                    .Where(x => x.audioClip != null && !string.IsNullOrEmpty(x.audioClip.name))
                    .Select(x => x.id)
                    .ToList();
                GenerateEnum("SoundID", listSound);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }


        private void GenerateEnum(string enumName, List<string> names)
        {
            if (names.Count == 0)
            {
                Logg.LogWarning("No sound names provided!");
                return;
            }

            string filePath = listSoundSo.filePathSoundID;
            StringBuilder sbExtensions = new StringBuilder();
            sbExtensions.AppendLine();

            StringBuilder sbEnum = new StringBuilder();
            sbEnum.AppendLine($"public enum {enumName}");
            sbEnum.AppendLine("{");

            HashSet<string> uniqueNames = new HashSet<string>(names.Select(n => n.Replace(" ", "_")));

            foreach (string name in uniqueNames)
            {
                sbEnum.AppendLine($"    {name},");
            }

            sbEnum.AppendLine("}");

            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, sbExtensions.ToString() + sbEnum.ToString());
                Logg.Log($"Updated Enum '{enumName}' at {filePath}");
            }
            else
            {
                File.WriteAllText(filePath, sbExtensions.ToString() + sbEnum.ToString());
                Logg.Log($"Created Enum '{enumName}' at {filePath}");
            }

            AssetDatabase.Refresh();
        }

        private void DrawGroupNames()
        {
            for (int i = 0; i < groupNames.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                string newName = EditorGUILayout.TextField($"Group {i + 1}", groupNames[i]);
                if (groupNames != null && newName != groupNames[i])
                {
                    groupNames[i] = newName;
                }

                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    groupNames.RemoveAt(i);
                    i--;
                    continue;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Group"))
            {
                groupNames.Add("NewGroup");
            }
        }

        private void DrawRowBorder()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, Color.gray);
        }

        private void DrawTableHeaders()
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.BeginHorizontal();
            DrawTableCell("    Audio Clip", 165, true);
            EditorGUILayout.LabelField("Group", GUILayout.Width(100));

            GUILayout.Space(-30);
            EditorGUILayout.LabelField("Type", GUILayout.Width(70));
            EditorGUILayout.LabelField("Volume", GUILayout.Width(95));

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Pitch", GUILayout.Width(75));

            GUILayout.Space(-10);
            EditorGUILayout.LabelField("Min", GUILayout.Width(75));
            GUILayout.Space(-50);
            EditorGUILayout.LabelField("Max", GUILayout.Width(75));


            GUILayout.Space(-20);
            EditorGUILayout.LabelField("Play", GUILayout.Width(75));
            GUILayout.Space(-30);
            EditorGUILayout.LabelField("Stop", GUILayout.Width(75));
            GUILayout.Space(-20);
            EditorGUILayout.LabelField("Remove", GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel -= 1;
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

        private void SortToType(ListSoundSO listSoundSo)
        {
            if (listSoundSo == null || listSoundSo.ListSoundInfos.Count == 0)
            {
                OSK.Logg.LogWarning("List Sound Infos is empty.");
                return;
            }

            listSoundSo.ListSoundInfos.Sort((a, b) => a.type.CompareTo(b.type));
            UnityEditor.EditorUtility.SetDirty(this);
        }

        private void SetIDForNameClip()
        {
            if (listSoundSo == null || listSoundSo.ListSoundInfos.Count == 0)
            {
                OSK.Logg.LogWarning("List Sound Infos is empty.");
                return;
            }

            for (int i = 0; i < listSoundSo.ListSoundInfos.Count; i++)
            {
                var name = listSoundSo.ListSoundInfos[i].audioClip.name;
                listSoundSo.ListSoundInfos[i].id = name;
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }

        public bool IsSoundExist(ListSoundSO listSoundSo, string soundName)
        {
            if (listSoundSo.ListSoundInfos == null || listSoundSo.ListSoundInfos.Count == 0)
                return false;

            foreach (var soundInfo in listSoundSo.ListSoundInfos)
            {
                if (soundInfo.audioClip.name == soundName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif