#if UNITY_EDITOR
using System.Collections.Generic; 
using UnityEngine;
using UnityEditor;

namespace OSK
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        // Các biến lưu trữ trạng thái mở/đóng của từng nhóm
        private bool showMusic = true;
        private bool showSFX = true;

        public override void OnInspectorGUI()
        {
            SoundManager soundManager = (SoundManager)target;

            // Draw default inspector for other fields
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("- Status Music : " + soundManager.isMusic.ToString());
            EditorGUILayout.LabelField("- Status SFX : " + soundManager.isSoundEffects.ToString());

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
                // Sổ nhóm Music
                showMusic = EditorGUILayout.Foldout(showMusic, "Music");
                if (showMusic)
                {
                    foreach (var playingSound in soundManager.GetListMusicInfos)
                    {
                        if (playingSound.soundInfo.type == SoundType.Music) // Giả sử có SoundType
                        {
                            DrawSoundInfo(playingSound);
                        }
                    }
                }

                // Sổ nhóm SFX
                showSFX = EditorGUILayout.Foldout(showSFX, "SFX");
                if (showSFX)
                {
                    foreach (var playingSound in soundManager.GetListMusicInfos)
                    {
                        if (playingSound.soundInfo.type == SoundType.SFX) // Giả sử có SoundType
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

        // Hàm phụ để vẽ thông tin về sound và các nút Play, Pause, Delete
        private void DrawSoundInfo(PlayingSound playingSound)
        {
            EditorGUILayout.BeginHorizontal();

            // Display sound name
            EditorGUILayout.LabelField(playingSound.audioSource.name, GUILayout.Width(200));

            // Play button
            if (GUILayout.Button("Play"))
            {
                playingSound.audioSource.Play();
            }

            // Pause button
            if (GUILayout.Button("Pause"))
            {
                playingSound.audioSource.Pause();
            }

            // Delete button
            if (GUILayout.Button("Delete"))
            {
                playingSound.audioSource.Stop();
                DestroyImmediate(playingSound.audioSource.gameObject);
                ((SoundManager)target).GetListMusicInfos.Remove(playingSound);
                ((SoundManager)target).GetListMusicInfos.RefreshList();
            }

            EditorGUILayout.EndHorizontal();
        }
        
        public static void FindSoundDataSOAssets()
        {
            // Lấy tất cả đường dẫn của các assets có kiểu SoundDataSO trong thư mục Assets
            string[] guids = AssetDatabase.FindAssets("t:SoundDataSO");
            List<SoundDataSO> soundDataAssets = new List<SoundDataSO>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SoundDataSO soundData = AssetDatabase.LoadAssetAtPath<SoundDataSO>(path);

                if (soundData != null)
                {
                    soundDataAssets.Add(soundData);
                }
            }

            // Hiển thị kết quả tìm thấy trong console
            if (soundDataAssets.Count > 0)
            {
                Debug.Log("Found " + soundDataAssets.Count + " SoundDataSO assets:");
                foreach (SoundDataSO data in soundDataAssets)
                {
                    Debug.Log(" - " + AssetDatabase.GetAssetPath(data));
                    Selection.activeObject = data;
                    EditorGUIUtility.PingObject(data);
                }
            }
            else
            {
                Debug.Log("No SoundDataSO assets found.");
            }
        }
    }
}
#endif
