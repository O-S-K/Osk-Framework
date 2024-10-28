using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SmartCopier
{
	public class ComponentWrapper
	{
		public Component Component { get; }
		public PropertyFilter PropertyFilter { get; }
		public PropertyProvider PropertyProvider { get; }
		public IEnumerable<PropertyWrapper> Properties { get; private set; }
		public bool Checked { get; set; }
		public bool FoldOut { get; set; }
		public Texture Icon { get; }

		public ComponentWrapper(Component component)
		{
			Component = component;
			PropertyFilter = new PropertyFilter();
			// "m_Script" is a serialized property of each custom Component, but there's no need for it to show up
			// in the SmartCopier interface or to copy it to a new Component.
			PropertyFilter.AddFilteredPropertyName("m_Script");
			PropertyFilter.AddFilteredAttribute<NoCopyAttribute>();
			PropertyProvider = new PropertyProvider(component, PropertyFilter);

			Checked = true;
			FoldOut = false;

			Icon = EditorGUIUtility.ObjectContent(component, component.GetType()).image;
			
			RefreshProperties();
		}

		/// Refresh the properties each time the PropertyProvider has changed.
		public void RefreshProperties()
		{
			Properties = GetProperties(Component).ToList();
		}

		private IEnumerable<PropertyWrapper> GetProperties(Component component)
		{
			var filteredProperties = PropertyProvider.GetValidProperties();
			return filteredProperties.Select(p => new PropertyWrapper(component, p));
		}

		public IEnumerable<Type> GetRequiredComponentTypes()
		{
			var attributes = Component.GetType().GetCustomAttributes(typeof(RequireComponent), true);
			return attributes
				.Cast<RequireComponent>()
				.SelectMany(attribute => new List<Type>() { attribute.m_Type0, attribute.m_Type1, attribute.m_Type2 })
				.Where(type => type != null);
		}
	}
}
