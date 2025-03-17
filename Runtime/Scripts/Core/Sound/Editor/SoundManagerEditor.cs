#if UNITY_EDITOR
using System.Collections.Generic; 
using UnityEngine;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        private bool showMusic = true;
        private bool showSFX = true;

        public override void OnInspectorGUI()
        {
            SoundManager soundManager = (SoundManager)target;

            // Draw default inspector for other fields
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Status Music : " + soundManager.IsMusic.ToString());
            EditorGUILayout.LabelField("- Status SFX : " + soundManager.IsSoundSFX.ToString());

            // Play button
            if (GUILayout.Button("Select Data SO"))
            {
                FindSoundDataSOAssets();
            } 

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Playing Sounds", EditorStyles.boldLabel, GUILayout.Width(400));

            if (soundManager.GetListSoundInfos != null &&
                (soundManager.GetListMusicInfos != null && soundManager.GetListMusicInfos.Count > 0))
            {
                // dropdown menu to select sound type
                showMusic = EditorGUILayout.Foldout(showMusic, "Music");
                if (showMusic)
                {
                    foreach (var playingSound in soundManager.GetListMusicInfos)
                    {
                        if (playingSound.SoundData.type == SoundType.MUSIC) // Giả sử có SoundType
                        {
                            DrawSoundInfo(playingSound);
                        }
                    }
                }

                //  dropdown menu to select sound type
                showSFX = EditorGUILayout.Foldout(showSFX, "SFX");
                if (showSFX)
                {
                    foreach (var playingSound in soundManager.GetListMusicInfos)
                    {
                        if (playingSound.SoundData.type == SoundType.SFX) // Giả sử có SoundType
                        {
                            DrawSoundInfo(playingSound);
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No sounds are currently playing.");
            }
        }

        //  Draw sound info
        private void DrawSoundInfo(PlayingSound playingSound)
        {
            EditorGUILayout.BeginHorizontal();

            // Display sound name
            EditorGUILayout.LabelField(playingSound.AudioSource.name, GUILayout.Width(200));

            // Play button
            if (GUILayout.Button("Play"))
            {
                playingSound.AudioSource.Play();
            }

            // Pause button
            if (GUILayout.Button("Pause"))
            {
                playingSound.AudioSource.Pause();
            }

            // Delete button
            if (GUILayout.Button("Delete"))
            {
                playingSound.AudioSource.Stop();
                DestroyImmediate(playingSound.AudioSource.gameObject);
                ((SoundManager)target).GetListMusicInfos.Remove(playingSound);
                ((SoundManager)target).GetListMusicInfos.RefreshList();
            }
            
            // volume slider
            playingSound.AudioSource.volume = EditorGUILayout.Slider(playingSound.AudioSource.volume, 0f, 2f);

            EditorGUILayout.EndHorizontal();
        }
        
        public static void FindSoundDataSOAssets()
        {
            // get all SoundDataSO assets
            string[] guids = AssetDatabase.FindAssets("t:SoundDataSO");
            List<ListSoundSO> soundDataAssets = new List<ListSoundSO>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ListSoundSO listSound = AssetDatabase.LoadAssetAtPath<ListSoundSO>(path);

                if (listSound != null)
                {
                    soundDataAssets.Add(listSound);
                }
            }

            // show all SoundDataSO assets
            if (soundDataAssets.Count > 0)
            {
                Logg.Log("Found " + soundDataAssets.Count + " SoundDataSO assets:");
                foreach (ListSoundSO data in soundDataAssets)
                {
                    Logg.Log(" - " + AssetDatabase.GetAssetPath(data));
                    Selection.activeObject = data;
                    EditorGUIUtility.PingObject(data);
                }
            }
            else
            {
                Logg.Log("No SoundDataSO assets found.");
            }
        }
    }
}
#endif
