using TMPro;
using UnityEngine;

namespace OSK
{
public class TextFading : MonoBehaviour
{
	private TextMeshProUGUI _text;

	[SerializeField]
	private float _duration = 1f;

	private Color _color = Color.white;

	private Color _colorEnd = Color.white;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
		_colorEnd.a = 0f;
	}

	private void Update()
	{
		_text.color = Color.Lerp(_colorEnd, _color, Parabole(Time.time / _duration % 1f));
	}

	private float Parabole(float x)
	{
		return -4f * x * x + 4f * x;
	}
}

}