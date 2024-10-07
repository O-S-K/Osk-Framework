using System.Collections;
using TMPro;
using UnityEngine;

namespace OSK
{
public class StreakAnimation : MonoBehaviour
{
	[SerializeField]
	private RectTransform _rt;

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private float _startScale = 1.5f;

	[SerializeField]
	private float _endScale = 0.6f;

	[SerializeField]
	private float _duration = 0.4f;

	private int _currentValue;

	private byte _key;

	public void SetValue(int initialValue, int newValue)
	{
		if (newValue != _currentValue)
		{
			_currentValue = newValue;
			if (newValue == 0)
			{
				_text.text = "× " + initialValue.ToString();
			}
			else
			{
				_text.text = "× " + initialValue.ToString() + " + " + newValue.ToString();
			}
			StartCoroutine(TweenScaleAnimation());
		}
	}

	private IEnumerator TweenScaleAnimation()
	{
		byte requirement = ++_key;
		float t = 0f;
		while (t < 1f && requirement == _key)
		{
			t += Time.unscaledDeltaTime / _duration;
			_rt.localScale = Vector3.Lerp(Vector3.one * _startScale, Vector3.one * _endScale, t);
			yield return null;
		}
		if (requirement == _key)
		{
			_rt.localScale = Vector3.one * _endScale;
		}
	}
}

}