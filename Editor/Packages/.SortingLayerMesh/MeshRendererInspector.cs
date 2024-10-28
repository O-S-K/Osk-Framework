using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( MeshRenderer ) )]
internal sealed class MeshRendererInspector : Editor
{
    private Editor             m_editor;
    private SerializedProperty m_sortingOrderProperty;
    private SerializedProperty m_sortingLayerIDProperty;

    public override void OnInspectorGUI()
    {
        if ( m_editor == null )
        {
            CreateCachedEditor
            (
                targetObjects: targets,
                editorType: System.Type.GetType( "UnityEditor.MeshRendererEditor, UnityEditor" ),
                previousEditor: ref m_editor
            );
        }

        m_sortingOrderProperty   ??= serializedObject.FindProperty( "m_SortingOrder" );
        m_sortingLayerIDProperty ??= serializedObject.FindProperty( "m_SortingLayerID" );

        m_editor.OnInspectorGUI();
        serializedObject.Update();

        SortingLayerEditorUtilityInternal.RenderSortingLayerFields
        (
            sortingOrder: m_sortingOrderProperty,
            sortingLayer: m_sortingLayerIDProperty
        );

        serializedObject.ApplyModifiedProperties();
    }
}
