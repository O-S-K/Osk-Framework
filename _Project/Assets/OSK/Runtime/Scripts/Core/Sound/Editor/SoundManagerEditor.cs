using OSK;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundManager soundManager = (SoundManager)target;

        // Draw default inspector for other fields
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("- Status Music : " + soundManager.isMusic.ToString());
        EditorGUILayout.LabelField("- Status SFX : " + soundManager.isSoundEffects.ToString());

        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Playing Sounds", EditorStyles.boldLabel, GUILayout.Width(400));

        if (soundManager.soundInfos != null && (soundManager.musicInfos != null && soundManager.musicInfos.Count > 0))
        {
            foreach (var playingSound in soundManager.musicInfos)
            {
                EditorGUILayout.BeginHorizontal();

                // Display sound name
                EditorGUILayout.LabelField(playingSound.soundInfo.audioClip.name);

                // Play button
                if (GUILayout.Button("Play"))
                {
                    playingSound.audioSource.Play();
                }
                
                // Play button
                if (GUILayout.Button("Pause"))
                {
                    playingSound.audioSource.Pause();
                } 

                // Delete button
                if (GUILayout.Button("Delete"))
                {
                    playingSound.audioSource.Stop();
                    DestroyImmediate(playingSound.audioSource.gameObject);
                    soundManager.musicInfos.Remove(playingSound); 
                    soundManager.musicInfos.RefreshList();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.LabelField("No sounds are currently playing.");
        }
    }
}