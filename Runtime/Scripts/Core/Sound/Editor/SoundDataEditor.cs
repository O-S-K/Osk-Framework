#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector.Editor;

namespace OSK
{
    [CustomEditor(typeof(ListSoundSO))]
    public class SoundDataEditor : OdinEditor
    {
        private bool showTable = true;
        private Dictionary<SoundType, bool> soundTypeFoldouts = new Dictionary<SoundType, bool>();
        ListSoundSO listSoundSo;

        public override void OnInspectorGUI()
        {
            listSoundSo = (ListSoundSO)target;
            DrawDefaultInspector();


            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField(
                "----------------------------------------------------------------------------------------------");

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

                            // type dropdown
                            soundData.type = (SoundType)EditorGUILayout.EnumPopup(soundData.type, GUILayout.Width(75));

                            // volume slider
                            GUILayout.Label(EditorGUIUtility.IconContent("d_AudioSource Icon"), GUILayout.Width(20), GUILayout.Height(20));
                            float newVolume = GUILayout.HorizontalSlider(soundData.volume, 0f, 1f, GUILayout.Width(50));
    
                            if (Mathf.Abs(newVolume - soundData.volume) > 0.01f) 
                            {
                                soundData.volume = newVolume;
                                soundData.SetVolume(newVolume);
                            }
                            GUILayout.Label(soundData.volume.ToString("F2"), GUILayout.Width(30)); // Hiển thị giá trị volume


                            GUI.enabled = soundData.audioClip != null && !soundData.IsPlaying();
                            GUI.color = soundData.IsPlaying() ? Color.green : Color.white;
                            
                            if (GUILayout.Button("Play", GUILayout.Width(50)))
                            {
                                soundData.Play();
                            }
                            GUI.color = Color.white;


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
                Debug.LogWarning("No sound names provided!");
                return;
            }
            /*
            sbExtensions.AppendLine("public static class SoundIDExtensions");
            sbExtensions.AppendLine("{");
            sbExtensions.AppendLine($"    public static implicit operator string({enumName} soundID) => soundID.ToString();");
            sbExtensions.AppendLine("}");
            sbExtensions.AppendLine(); // Dòng trống*/

            string filePath = listSoundSo.filePathSoundID;
            StringBuilder sbExtensions = new StringBuilder();
            StringBuilder sbEnum = new StringBuilder();
            sbEnum.AppendLine($"public enum {enumName}");
            sbEnum.AppendLine("{");

            HashSet<string> uniqueNames = new HashSet<string>(names.Select(n => n.Replace(" ", "_")));

            foreach (string name in uniqueNames)
            {
                sbEnum.AppendLine($"    {name},");
            }

            sbEnum.AppendLine("}");

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, sbExtensions.ToString() + sbEnum.ToString());
                Debug.Log($"Created Enum '{enumName}' at {filePath}");
            }
            else
            {
                string oldContent = File.ReadAllText(filePath);
                bool hasExtensions = oldContent.Contains("public static class SoundIDExtensions");

                string pattern = @"public\s+enum\s+" + enumName + @"\s*\{([\s\S]*?)\}";
                Match match = Regex.Match(oldContent, pattern);

                if (match.Success)
                {
                    string existingValues = match.Groups[1].Value;
                    HashSet<string> existingEnumSet = new HashSet<string>(
                        Regex.Matches(existingValues, @"\s*(\w+),").Select(m => m.Groups[1].Value)
                    );

                    foreach (string name in uniqueNames)
                    {
                        existingEnumSet.Add(name);
                    }

                    StringBuilder newEnumContent = new StringBuilder();
                    newEnumContent.AppendLine($"public enum {enumName}");
                    newEnumContent.AppendLine("{");
                    foreach (string value in existingEnumSet)
                    {
                        newEnumContent.AppendLine($"    {value},");
                    }

                    newEnumContent.AppendLine("}");

                    string updatedContent = Regex.Replace(oldContent, pattern, newEnumContent.ToString());

                    if (!hasExtensions)
                    {
                        updatedContent = sbExtensions.ToString() + updatedContent;
                    }

                    File.WriteAllText(filePath, updatedContent);
                    Debug.Log($"Updated Enum '{enumName}' at {filePath}");
                }
                else
                {
                    Debug.LogWarning("Enum structure not found in file, overwriting...");
                    File.WriteAllText(filePath, sbExtensions.ToString() + sbEnum.ToString());
                }
            }

            AssetDatabase.Refresh();
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
            EditorGUILayout.LabelField("Actions", GUILayout.Width(200));
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