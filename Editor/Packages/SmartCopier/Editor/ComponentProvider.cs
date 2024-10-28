using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmartCopier
{
	public class ComponentProvider
	{
		private readonly HashSet<Type> _filteredComponentTypes = new HashSet<Type>();

		/// Filter Components of type T.
		public void AddFilteredComponentType<T>() where T : Component
		{
			_filteredComponentTypes.Add(typeof(T));
		}

		/// Remove filtered Component of type T.
		public bool RemoveFilteredComponentType<T>() where T : Component
		{
			return _filteredComponentTypes.Remove(typeof(T));
		}

		public IEnumerable<ComponentWrapper> GetFilteredComponents(GameObject gameObject)
		{
			IEnumerable<Component> allComponents = gameObject.GetComponents<Component>();
			return allComponents
				.Where(c => !_filteredComponentTypes.Contains(c.GetType()))
				.Select(c => new ComponentWrapper(c))
				.ToList();
		}
	}
}
