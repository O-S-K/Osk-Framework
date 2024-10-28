using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace SmartCopier
{
	public class PropertyProvider
	{
		private readonly SerializedObject _serializedObject;
		private readonly PropertyFilter _propertyFilter;

		public PropertyProvider(UnityEngine.Object obj, PropertyFilter propertyFilter)
		{
			_serializedObject = new SerializedObject(obj);
			_propertyFilter = propertyFilter;
		}

		public IEnumerable<SerializedProperty> GetValidProperties()
		{
			Type objectType = _serializedObject.targetObject.GetType();
			var allProperties = GetAllSerializedProperties();
			return allProperties.Where(property => IsPropertyNotFiltered(property, objectType));
		}

		private bool IsPropertyNotFiltered(SerializedProperty property, Type targetObjectType)
		{
			return !_propertyFilter.IsPropertyFiltered(property, targetObjectType);
		}

		private IEnumerable<SerializedProperty> GetAllSerializedProperties()
		{
			var properties = new List<SerializedProperty>();
			var iterator = _serializedObject.GetIterator();
			bool getChildren = true;
			while (iterator.NextVisible(getChildren))
			{
				getChildren = false;
				properties.Add(iterator.Copy());
			}
			return properties;
		}
	}
}