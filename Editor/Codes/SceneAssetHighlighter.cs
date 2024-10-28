using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

[InitializeOnLoad]
internal static class SceneAssetHighlighter
{
    private static readonly Color COLOR = new Color( 0.1f, 0.6f, 0.1f, 0.5f );
		
    static SceneAssetHighlighter()
    {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI( string guid, Rect selectionRect )
    {
        var path = AssetDatabase.GUIDToAssetPath( guid );

        var activeScene = SceneManager.GetActiveScene();
        var filename    = Path.GetFileNameWithoutExtension( path );

        if ( activeScene.name != filename ) return;

        var color = GUI.color;
        GUI.color = COLOR;
        GUI.DrawTexture( selectionRect, EditorGUIUtility.whiteTexture );
        GUI.color = color;
    }
}