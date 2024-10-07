using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OSK
{
    public class ProgressBar : MonoBehaviour
    {
        public Slider slider;
        public Image filler;
        public TextMeshProUGUI progressText;

        public bool showPercentage;

        private int _maxVal;
        private float _currentVal;

        private void Start()
        {
            slider.value = _currentVal;
        }

        private void Update()
        {
            slider.value = Mathf.Lerp(slider.value, _currentVal, Time.deltaTime * 10.0f);
        }

        public void SetMaxValue(int maxValue)
        {
            _maxVal = maxValue;
            slider.maxValue = maxValue;
            SetProgress(0);
        }

        public void SetProgress(int val)
        {
            _currentVal = val / 100.0f;

            if (progressText)
            {
                string v = val.ToString();
                if (showPercentage)
                {
                    v += "%";
                }
                progressText.text = v;
            }
        }
    }
}