using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartCopier
{
	public class CopyContext
	{
		public class SourceObjectContext
		{
			public int GameObjectId { get; internal set; }
			public string GameObjectName { get; internal set; }
		}
		
		public SourceObjectContext SourceGameObjectContext { get; }
		public ComponentProvider ComponentProvider { get; }
		public IEnumerable<ComponentWrapper> Components { get; private set; }

		public CopyContext(GameObject objectToCopyFrom)
		{
			SourceGameObjectContext = new SourceObjectContext
			{
				GameObjectId = objectToCopyFrom.GetInstanceID(),
				GameObjectName = objectToCopyFrom.name
			};
			ComponentProvider = new ComponentProvider();
			// EXAMPLE: If you want to disable copying an object's Transform component by default, uncomment the line below
			// ComponentProvider.AddFilteredComponentType<Transform>(); // This works for any type of component.
			RefreshComponents(objectToCopyFrom);
		}

		/// Refresh the components each time the ComponentProvider has changed.
		private void RefreshComponents(GameObject objectToCopyFrom)
		{
			Components = ComponentProvider.GetFilteredComponents(objectToCopyFrom);
		}

		/// Paste all components and their checked properties into the target GameObject.
		public void PasteComponents(GameObject targetGameObject, CopyMode copyMode)
		{
			var orderedComponents = SortByRequiredFirst(Components.Where(c => c.Checked));
			foreach (var wrapper in orderedComponents)
			{
				Type componentType = wrapper.Component.GetType();
				if (copyMode == CopyMode.PasteAsNew)
				{
					var newComponent = Undo.AddComponent(targetGameObject, componentType);
					// Might return null if component already exists.
					if (newComponent != null)
					{
						CopyComponentWithUndo(wrapper, newComponent);
					}
				}
				else
				{
					var otherComponent = targetGameObject.GetComponent(componentType);
					if (otherComponent == null)
					{
						otherComponent = Undo.AddComponent(targetGameObject, componentType);
					}

					// otherComponent can still be null if adding the component failed for any reason.
					if (otherComponent != null)
					{
						CopyComponentWithUndo(wrapper, otherComponent);
					}
				}
			}
		}

		private void CopyComponentWithUndo(ComponentWrapper source, Component target)
		{
			Undo.RecordObject(target, "Copy component properties");
			CopyComponent(source, target);
			Undo.FlushUndoRecordObjects();
		}

		private void CopyComponent(ComponentWrapper source, Component target)
		{
			CopyProperties(source, target);
		}

		private void CopyProperties(ComponentWrapper source, Component target)
		{
			var targetSerializedObject = new SerializedObject(target);
			foreach (var property in source.Properties.Where(p => p.Checked))
			{
				targetSerializedObject.CopyFromSerializedProperty(property.SerializedProperty);
			}
			targetSerializedObject.ApplyModifiedProperties();
		}

		/// Puts Components that are required by other Components (using RequireComponent) at the top of the list when copying.
		/// This avoid Unity automatically adding an instance of the required component when copying both the Component + its required Component.
		/// NOTE: This only goes one level deep (chains of required components should be avoided) - this is not a full topological sort.
		private class ComponentDependencyComparer : IComparer<ComponentWrapper>
		{
			public int Compare(ComponentWrapper c1, ComponentWrapper c2)
			{
				var c1RequiredTypes = c1.GetRequiredComponentTypes();
				if (c1RequiredTypes.Contains(c2.Component.GetType()))
				{
					return 1;
				}
				var c2RequiredTypes = c2.GetRequiredComponentTypes();
				if (c2RequiredTypes.Contains(c1.Component.GetType()))
				{
					return -1;
				}
				return 0;
			}
		}

		private IEnumerable<ComponentWrapper> SortByRequiredFirst(IEnumerable<ComponentWrapper> components)
		{
			return components.OrderBy(component => component, new ComponentDependencyComparer());
		}
	}
}
