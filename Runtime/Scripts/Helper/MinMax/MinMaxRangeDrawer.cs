using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
 
// [MinMaxRange(0, 1)]
// public MinMax _minMaxRange = new MinMax(0.1f, 0.9f);
public class MinMaxRangeAttribute : PropertyAttribute {
	
	public float minLimit;
	public float maxLimit;
	
	public MinMaxRangeAttribute (float minLimit, float maxLimit) {
		
		this.minLimit = minLimit;
		this.maxLimit = maxLimit;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer {
	
	const int numWidth = 50;
	const int padding = 5;
	
	MinMaxRangeAttribute minMaxAttribute { get { return (MinMaxRangeAttribute)attribute; } }
	
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
		
		EditorGUI.BeginProperty (position, label, prop);
		
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
		
		Rect minRect 	= new Rect (position.x, position.y, numWidth, position.height);
		Rect sliderRect = new Rect (minRect.x + minRect.width + padding, position.y, position.width - numWidth*2 - padding*2, position.height);
		Rect maxRect 	= new Rect (sliderRect.x + sliderRect.width + padding, position.y, numWidth, position.height);

		SerializedProperty minProp = prop.FindPropertyRelative("min");
		SerializedProperty maxProp = prop.FindPropertyRelative("max");

		float min = minProp.floatValue;
		float max = maxProp.floatValue;
		float minLimit = minMaxAttribute.minLimit;
		float maxLimit = minMaxAttribute.maxLimit;

		min = Mathf.Clamp(EditorGUI.FloatField (minRect, min), minLimit, max);
		max = Mathf.Clamp(EditorGUI.FloatField (maxRect, max), min, maxLimit);
		EditorGUI.MinMaxSlider (sliderRect, ref min, ref max, minLimit, maxLimit);

		minProp.floatValue = min;
		maxProp.floatValue = max;
		
		EditorGUI.EndProperty ();
	}
}
#endif