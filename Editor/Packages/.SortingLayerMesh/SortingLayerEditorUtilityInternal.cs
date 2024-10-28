using System;
using UnityEditor;
using UnityEngine;

public static class SortingLayerEditorUtilityInternal
{
    private static readonly Type TYPE = typeof(Editor)
        .Assembly.GetType("UnityEditor.SortingLayerEditorUtility");

    public static void RenderSortingLayerFields
    (
        SerializedProperty sortingOrder,
        SerializedProperty sortingLayer
    )
    {
        var methodInfo = TYPE.GetMethod
        (
            name: "RenderSortingLayerFields",
            types: new[]
            {
                typeof(SerializedProperty),
                typeof(SerializedProperty)
            }
        );

        methodInfo!.Invoke
        (
            obj: null,
            parameters: new object[] { sortingOrder, sortingLayer }
        );
    }

    public static void RenderSortingLayerFields
    (
        Rect r,
        SerializedProperty sortingOrder,
        SerializedProperty sortingLayer
    )
    {
        var methodInfo = TYPE.GetMethod
        (
            name: "RenderSortingLayerFields",
            types: new[]
            {
                typeof(Rect),
                typeof(SerializedProperty),
                typeof(SerializedProperty)
            }
        );

        methodInfo!.Invoke
        (
            obj: null,
            parameters: new object[] { r, sortingOrder, sortingLayer }
        );
    }
}