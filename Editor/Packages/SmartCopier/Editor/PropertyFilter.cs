using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace SmartCopier
{
	public class PropertyFilter
	{
		private readonly HashSet<Type> _filteredAttributes = new HashSet<Type>();
		private readonly HashSet<string> _filteredPropertyNames = new HashSet<string>();

		public bool IsPropertyFiltered(SerializedProperty property, Type targetObjectType)
		{
			FieldInfo field = targetObjectType.GetField(property.propertyPath);
			return IsFilteredByPropertyName(property) ? true : field != null && IsFilteredByAttribute(field);
		}

		/// <summary>
		/// Add a filtered property name. Properties with this name will not be copied.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property in your scripts, e.g. "m_SomeProperty".</param>
		public void AddFilteredPropertyName(string propertyName)
		{
			_filteredPropertyNames.Add(propertyName);
		}

		/// <summary>
		/// Remove a filtered property name.
		/// </summary>
		/// <param name="propertyName">The property name to be removed.</param>
		/// <returns>Whether the filtered property name was removed successfully.</returns>
		public bool RemoveFilteredPropertyName(string propertyName)
		{
			return _filteredPropertyNames.Remove(propertyName);
		}

		/// <summary>
		/// Add an attribute of type T to the PropertyFilter.
		/// Properties that have an attribute of type T will not get copied.
		/// </summary>
		/// <typeparam name="T">The type of attribute to filter, e.g. NoCopy.</typeparam>
		public void AddFilteredAttribute<T>() where T : Attribute
		{
			_filteredAttributes.Add(typeof(T));
		}

		/// <summary>
		/// Remove an attribute of type T from the PropertyFilter.
		/// </summary>
		/// <typeparam name="T">The Type of attribute filter to remove.</typeparam>
		/// <returns>Whether the attribute was successfully removed.</returns>
		public bool RemoveFilteredAttribute<T>() where T : Attribute
		{
			return _filteredAttributes.Remove(typeof(T));
		}

		private bool IsFilteredByPropertyName(SerializedProperty property)
		{
			return _filteredPropertyNames.Any(name => property.name == name);
		}

		private bool IsFilteredByAttribute(FieldInfo field)
		{
			return _filteredAttributes.Any(att => field.GetCustomAttributes(att, false).Length > 0);
		}
	}
}
