using TMPro;
using UnityEngine;

namespace OSK
{
    public class TextFading : MonoBehaviour
    {
        [SerializeField] private float _duration = 1f;
        [SerializeField] private bool _fadeOut = true;

        private TextMeshProUGUI _text;

        private Color _color = Color.white;

        private Color _colorEnd = Color.white;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _colorEnd.a = 0f;
        }

        private void Update()
        {
            if (_fadeOut)
                _text.color = Color.Lerp(_color, _colorEnd, Mathf.Sin(Time.time / _duration % 1f));
            else
                _text.color = Color.Lerp(_colorEnd, _color, Parabole(Time.time / _duration % 1f));
        }

        private float Parabole(float x)
        {
            return -4f * x * x + 4f * x;
        }
    }
}