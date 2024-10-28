using System;
using UnityEditor;

namespace SmartCopier
{
	public class PropertyWrapper
	{
		private class SerializedPropertyRef
		{
			private readonly UnityEngine.Object _unityObj;
			private readonly SerializedObject _cachedSerializedObject;
			private readonly string _propertyPath;

			public SerializedPropertyRef(UnityEngine.Object unityObj, SerializedProperty prop)
			{
				_unityObj = unityObj;
				_propertyPath = prop.propertyPath;
				_cachedSerializedObject = new SerializedObject(unityObj);
			}

			public SerializedProperty GetProperty()
			{
				try
				{
					var serializedObj = new SerializedObject(_unityObj);
					return serializedObj.FindProperty(_propertyPath);
				}
				// If the Unity Object has been destroyed, an ArgumentException will be thrown when trying to create
				// the SerializedObject. In this case, we take the cached serialized object instead.
				// This cached SerializedObject might not have all fully updated properties, but it is better than failing.
				catch (ArgumentException)
				{
					return _cachedSerializedObject.FindProperty(_propertyPath);
				}
			}
		}

		private readonly SerializedPropertyRef _propertyRef;

		public SerializedProperty SerializedProperty => _propertyRef.GetProperty();
		public string Name { get; }
		public bool Checked { get; set; }

		public PropertyWrapper(UnityEngine.Object unityObj, SerializedProperty property)
		{
			_propertyRef = new SerializedPropertyRef(unityObj, property);
			Name = property.displayName;
			Checked = true;
		}
	}
}

