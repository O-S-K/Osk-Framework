using System;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

namespace OSK
{
    public class MoveAlongPathEditorWindow : EditorWindow
    {
        private MoveAlongPathProvider moveAlongPathProvider;
        public  int indexPathSelect;


        [MenuItem("OSK-Framework/Path Editor/Move Along Path Editor")]
        public static void ShowWindow()
        {
            GetWindow<MoveAlongPathEditorWindow>("Path Editor");
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            if (moveAlongPathProvider == null)
            {
                EditorGUILayout.HelpBox("Select a MoveAlongPathProvider to edit.", MessageType.Info);
                return;
            }

            GUILayout.Space(20);
            GUILayout.Label("Path Editor", EditorStyles.boldLabel);


            if (moveAlongPathProvider.paths.Count > 0)
            {
                GUILayout.Label("Path Points:", EditorStyles.label);

                for (int i = 0; i < moveAlongPathProvider.paths.Count; i++)
                {
                    Path path = moveAlongPathProvider.paths[i];
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label($"Index {i}", EditorStyles.label);
                    path.index = i;
                    path.position = EditorGUILayout.Vector3Field("Position", path.position);
                    path.rotation =
                        Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", path.rotation.eulerAngles));

                    if (GUILayout.Button("Remove"))
                    {
                        moveAlongPathProvider.RemovePathPoint(path);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            
            GUILayout.Space(20);
            GUILayout.Label("Path Selection " + indexPathSelect, EditorStyles.label);
            GUILayout.Space(20);

            if (GUILayout.Button("Add Path Point"))
            {
                moveAlongPathProvider.AddPathPoint();
            }

            if (GUILayout.Button("Clean All"))
            {
                moveAlongPathProvider.paths.Clear();
            }

            moveAlongPathProvider.isClosedPath =
                EditorGUILayout.Toggle("Is Closed Path", moveAlongPathProvider.isClosedPath);
            moveAlongPathProvider.typePath =
                (PathType)EditorGUILayout.EnumPopup("Path Type", moveAlongPathProvider.typePath);
            moveAlongPathProvider.pathMode =
                (PathMode)EditorGUILayout.EnumPopup("Path Mode", moveAlongPathProvider.pathMode);
            Repaint();
        }

        private void OnSelectionChange()
        {
            moveAlongPathProvider = Selection.activeGameObject?.GetComponent<MoveAlongPathProvider>();
            Repaint();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (moveAlongPathProvider == null || moveAlongPathProvider.paths.Count == 0)
                return;
            Event e = Event.current;

            // Existing code for path editing
            for (int i = 0; i < moveAlongPathProvider.paths.Count; i++)
            {
                Path path = moveAlongPathProvider.paths[i];

                float handleSize = HandleUtility.GetHandleSize(path.position) * 0.2f; 
                Handles.color = Color.blue; 
                
                Vector3 newPosition = Handles.FreeMoveHandle(
                    path.position,
                    handleSize,
                    Vector3.zero,
                    Handles.SphereHandleCap 
                );
                
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    indexPathSelect = i + 1;
                }
                

                if (newPosition != path.position)
                {
                    Undo.RecordObject(moveAlongPathProvider, "Move Path Point");
                    path.position = newPosition;
                } 
            }
            
                
            if (e.type == EventType.MouseDown && e.button == 2 && indexPathSelect != -1)
            {
                indexPathSelect = -1;
                moveAlongPathProvider.RemovePathPoint(moveAlongPathProvider.paths[indexPathSelect]);
                e.Use(); 
            }
           

            // Handle point addition on Ctrl + Left Click
            if (e.type == EventType.MouseDown && e.button == 0 && e.control)
            {
                Vector3 mousePos = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                AddPathPointAt(mousePos);
                e.Use(); // Consume the event to prevent further processing
            }



            // Drawing paths
            var paths = moveAlongPathProvider.paths;
            if (paths == null || paths.Count == 0)
                return;

            switch (moveAlongPathProvider.pathMode)
            {
                case PathMode.Ignore:
                    break;
                case PathMode.Full3D:
                    break;
                case PathMode.TopDown2D:

                    for (int i = 0; i < moveAlongPathProvider.paths.Count; i++)
                    {
                        moveAlongPathProvider.paths[i].position = new Vector3(
                            moveAlongPathProvider.paths[i].position.x,
                            0,
                            moveAlongPathProvider.paths[i].position.z
                        );
                    }

                    break;
                case PathMode.Sidescroller2D:
                    for (int i = 0; i < moveAlongPathProvider.paths.Count; i++)
                    {
                        moveAlongPathProvider.paths[i].position = new Vector3(
                            moveAlongPathProvider.paths[i].position.x,
                            moveAlongPathProvider.paths[i].position.y,
                            0
                        );
                    }
                    break;
            }

            // Draw the start and end points with unique colors
            Handles.color = Color.green;
            Handles.SphereHandleCap(0, paths[0].position, Quaternion.identity, 1f, EventType.Repaint);

            Handles.color = Color.red;
            Handles.SphereHandleCap(0, paths[^1].position, Quaternion.identity, 1f, EventType.Repaint);

            // Draw lines for the path based on the selected path type
            if (moveAlongPathProvider.typePath == PathType.Linear)
            {
                Handles.color = Color.magenta;
                for (int j = 0; j < paths.Count - 1; j++)
                {
                    Handles.DrawLine(paths[j].position, paths[j + 1].position);
                }

                // Draw a line connecting the last point to the first if isClosedPath is enabled
                if (moveAlongPathProvider.isClosedPath)
                {
                    Handles.DrawLine(paths[paths.Count - 1].position, paths[0].position);
                }
            }
            else
            {
                for (int j = 0; j < paths.Count - 1; j++)
                {
                    Handles.color = Color.magenta;

                    Vector3 p0 = paths[Mathf.Max(j - 1, 0)].position;
                    Vector3 p1 = paths[j].position;
                    Vector3 p2 = paths[Mathf.Min(j + 1, paths.Count - 1)].position;
                    Vector3 p3 = paths[Mathf.Min(j + 2, paths.Count - 1)].position;
                    DrawCatmullRomCurve(p0, p1, p2, p3, Handles.color);
                }

                // Draw a curve connecting the last point to the first if isClosedPath is enabled
                if (moveAlongPathProvider.isClosedPath)
                {
                    Vector3 p0 = paths[^2].position;
                    Vector3 p1 = paths[^1].position;
                    Vector3 p2 = paths[0].position;
                    Vector3 p3 = paths[1].position;
                    DrawCatmullRomCurve(p0, p1, p2, p3, Handles.color);
                }
            }

            if (paths == null || paths.Count == 0)
                return;
            SceneView.RepaintAll();
        }

        private void AddPathPointAt(Vector3 position)
        {
            if (moveAlongPathProvider != null)
            {
                moveAlongPathProvider.AddPathPoint(position, Quaternion.identity);
                Repaint();
            }
        }

        private void RemovePathPointAt(Path path)
        {
            if (moveAlongPathProvider != null)
            {
                moveAlongPathProvider.RemovePathPoint(path);
                Repaint();
            }
        }

        private void DrawCatmullRomCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color, int segments = 20)
        {
            Vector3 previousPoint = p1;
            Handles.color = color;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector3 pointOnCurve = OSK.MathUtils.CalculateCatmullRom(p0, p1, p2, p3, t);
                Handles.DrawLine(previousPoint, pointOnCurve);
                previousPoint = pointOnCurve;
            }
        }

        private void DrawCubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color color,
            int segments = 20)
        {
            Vector3 previousPoint = p0;
            Handles.color = color;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector3 pointOnCurve = OSK.MathUtils.CalculateCubicBezierPoint(t, p0, p1, p2, p3);
                Handles.DrawLine(previousPoint, pointOnCurve);
                previousPoint = pointOnCurve;
            }
        }
    }
}