#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class FilterTexture : EditorWindow
{
    public Dictionary<Texture2D, bool> m_Object = new Dictionary<Texture2D, bool>();
    public List<Texture2D> m_Textures;
    public string[] m_Textures_Guild;
    SerializedObject serializedObject;
    Vector2 scrollPos;

    [MenuItem("OSK-FrameWorld/Tools/Filter Textures And Remove Unused Textures")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FilterTexture));
    }

    void OnEnable()
    {
        m_Textures = new List<Texture2D>();
        titleContent = new GUIContent("Filter Textures");
        serializedObject = new SerializedObject(this);
    }


    void OnSelectionChange()
    {
        Repaint();
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 100f;
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        GUILayout.Space(6f);
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Space(6f);

        if (Selection.activeObject != null)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out var guid, out long file);
            EditorGUILayout.TextField("guild", !string.IsNullOrEmpty(guid) ? guid : "");
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUIStyle.none, GUI.skin.verticalScrollbar);
        GUILayout.BeginVertical();
        GUILayout.Space(10f);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Textures"), true);
        serializedObject.ApplyModifiedProperties();
        m_Textures_Guild = new string[m_Textures.Count];
        GUILayout.EndVertical();
        GUILayout.Space(6f);
        GUILayout.EndScrollView();

        if (GUILayout.Button("Find"))
        {
            var paths = AssetDatabase.GetAllAssetPaths();
            for (int j = 0; j < paths.Length; j++)
            {
                if (j >= paths.Length) break;
                var s = paths[j];
                Find(s);
            }


            foreach (var v in m_Object)
            {
                if (!v.Value)
                {
                    File.Delete(AssetDatabase.GetAssetPath(v.Key));
                }
            }
            AssetDatabase.Refresh();
        }

        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }



    public void Find(string s)
    {
        var t = s.Split('.')[s.Split('.').Length - 1];
        if (s.StartsWith("Assets") && (t == "prefab" || t == "unity" || t == "asset" || t == "mat" || t == "anim"))
        {
            var tmp = File.ReadAllLines(s);
            foreach (var v in tmp)
            {
                if (v.Contains(" guid:"))
                {
                    for (int j = 0; j < m_Textures.Count; j++)
                    {
                        if (!m_Object.ContainsKey(m_Textures[j]))
                        {
                            m_Object.Add(m_Textures[j], false);
                        }

                        if (!m_Object[m_Textures[j]])
                        {
                            var guid = m_Textures_Guild[j];
                            if (string.IsNullOrEmpty(guid))
                            {
                                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(m_Textures[j], out guid, out long file);
                                m_Textures_Guild[j] = guid;
                            }

                            if (v.Contains($" guid: {guid}"))
                            {
                                m_Object[m_Textures[j]] = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif
