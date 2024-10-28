using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartCopier
{
	public class SmartCopier : EditorWindow
	{
		public GUIStyle HeaderLabelStyle { get; private set; }
		public GUIStyle ComponentLabelStyle { get; private set; }
		public GUIStyle ComponentRowStyle { get; private set; }

		private const int Order = 10000000;

		private CopyContext _context;

		private Vector2 _scrollLocation;

		[MenuItem("CONTEXT/Component/Smart Copy Components", false, Order)]
		private static void CreateWindow(MenuCommand menuCommand)
		{
			Component targetComponent = (Component)menuCommand.context;;
			InitializeWindow(targetComponent.gameObject);
		}

		[MenuItem("GameObject/Smart Copy Components", true)]
		private static bool CanCopy(MenuCommand menuCommand)
		{
			return Selection.activeGameObject != null;
		}

		[MenuItem("GameObject/Smart Copy Components", false, -1)]
		private static void CopyFromGameObject(MenuCommand menuCommand)
		{
			InitializeWindow((GameObject) menuCommand.context);
		}

		[MenuItem("Assets/Smart Copy Components", false, -1)]
		private static void CopyFromGameObjectAsset()
		{
			InitializeWindow(Selection.activeGameObject);
		}

		[MenuItem("Assets/Smart Copy Components", true)]
		private static bool CopyFromGameObjectAssetValidation()
		{
			return Selection.activeGameObject != null;
		}

		private static void InitializeWindow(GameObject targetObject)
		{
			var window = GetWindow<SmartCopier>(false, "SmartCopier");
			window.Initialize(targetObject);
		}

		private void Initialize(GameObject objectToCopyFrom)
		{
			_context = new CopyContext(objectToCopyFrom);
		}

		// Repaint the GUI when the user's GameObject selection changes.
		private void OnSelectionChange()
		{
			Repaint();
		}

		protected void OnGUI()
		{
			if (_context == null)
			{
				GetWindow<SmartCopier>().Close();
				return;
			}

			// Create styles if necessary.
			CreateStyles();

			// BEGIN SCROLL
			_scrollLocation = GUILayout.BeginScrollView(_scrollLocation, ComponentRowStyle);

			// Header label
			GUILayout.Label(
				$"Select which of <b>{_context.SourceGameObjectContext.GameObjectName}'s</b> Components and properties to copy.",
				HeaderLabelStyle
			);

			DrawComponents(_context.Components);

			// Get target GameObjects
			IEnumerable<GameObject> objectsToPasteTo = GetTargetGameObjects();

			// END SCROLL
			GUILayout.EndScrollView();

			// Bottom row - target GameObjects
			GUILayout.BeginVertical(ComponentRowStyle);
			DrawSelectedGameObjects(objectsToPasteTo);

			if (objectsToPasteTo.Any())
			{
				DrawPasteButtons(objectsToPasteTo);
			}
			GUILayout.EndVertical();
		}

		private IEnumerable<GameObject> GetTargetGameObjects()
		{
			IEnumerable<GameObject> selectedObjects = Selection.gameObjects;
			return selectedObjects
				.Where(go => go.GetInstanceID() != _context.SourceGameObjectContext.GameObjectId)
				.ToList();
		}

		private void DrawComponents(IEnumerable<ComponentWrapper> components)
		{
			foreach (var wrapper in components)
			{
				DrawComponent(wrapper);
			}
		}

		private void DrawComponent(ComponentWrapper wrapper)
		{
			Type componentType = wrapper.Component.GetType();

			// Header row
			GUILayout.BeginVertical(ComponentRowStyle);
			string componentName = ObjectNames.NicifyVariableName(componentType.Name);
			GUIContent content = new GUIContent(componentName, wrapper.Icon);

			// Checkbox
			wrapper.Checked = EditorGUILayout.ToggleLeft(content, wrapper.Checked, wrapper.Checked ? ComponentLabelStyle : GUI.skin.label);
			if (wrapper.Checked)
			{
				// Draw property foldout if there are any serialized properties.
				if (wrapper.Properties.Any())
				{
					wrapper.FoldOut = EditorGUILayout.Foldout(wrapper.FoldOut, componentName + " properties and fields");
					if (wrapper.FoldOut)
					{
						++EditorGUI.indentLevel;
						DrawProperties(wrapper.Properties);
						--EditorGUI.indentLevel;
					}
				}
				else
				{
					GUILayout.Label(componentName + " has no serialized properties.");
				}
			}
			GUILayout.EndVertical();
		}

		private void DrawProperties(IEnumerable<PropertyWrapper> properties)
		{
			foreach (var property in properties)
			{
				//EditorGUILayout.BeginHorizontal();
				//EditorGUILayout.LabelField(property.Name);
				property.Checked = EditorGUILayout.ToggleLeft(property.Name, property.Checked);
				//EditorGUI.BeginDisabledGroup(true);
				//EditorGUILayout.PropertyField(property.SerializedProperty, GUIContent.none);
				//EditorGUI.EndDisabledGroup();
				//EditorGUILayout.EndHorizontal();
			}
		}

		private void DrawSelectedGameObjects(IEnumerable<GameObject> selectedObjects)
		{
			GUILayout.Label("Paste components into these GameObjects:");
			if (selectedObjects.Any())
			{
				GUI.enabled = false;
				foreach (GameObject go in selectedObjects)
				{
					EditorGUILayout.ObjectField(go, go.GetType(), true);
				}
				GUI.enabled = true;
			}
			else
			{
				EditorGUILayout.HelpBox("Select one or more GameObjects.", MessageType.Info);
			}
		}

		private void DrawPasteButtons(IEnumerable<GameObject> targetObjects)
		{
			GUILayout.BeginHorizontal();
			var height = GUILayout.Height(30);
			if (GUILayout.Button(new GUIContent("Paste And Replace", "Create new components with the selected properties."), height))
			{
				PasteTo(targetObjects, CopyMode.ReplaceValues);
			}
			else if (GUILayout.Button(new GUIContent("Paste As New", "Replace existing values, or create new."), height))
			{
				PasteTo(targetObjects, CopyMode.PasteAsNew);
			}
			GUILayout.EndHorizontal();
		}

		private void PasteTo(IEnumerable<GameObject> targetObjects, CopyMode copyMode)
		{
			foreach (var targetGameObject in targetObjects)
			{
				_context.PasteComponents(targetGameObject, copyMode);
			}
		}

		private void CreateStyles()
		{
			const int padding = 8;

			if (HeaderLabelStyle == null)
			{
				HeaderLabelStyle = new GUIStyle(GUI.skin.label);
				HeaderLabelStyle.richText = true;
				HeaderLabelStyle.wordWrap = true;
			}

			if (ComponentLabelStyle == null)
			{
				ComponentLabelStyle = new GUIStyle(GUI.skin.label);
				ComponentLabelStyle.fontStyle = FontStyle.Bold;
			}

			if (ComponentRowStyle == null)
			{
				ComponentRowStyle = new GUIStyle(GUI.skin.box);
				ComponentRowStyle.padding = new RectOffset(padding, padding, padding, padding);
				ComponentRowStyle.margin = ComponentRowStyle.padding;
			}
		}
	}
}
