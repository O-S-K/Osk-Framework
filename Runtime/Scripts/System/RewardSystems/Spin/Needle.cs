using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Needle : MonoBehaviour
    {
        public RectTransform RectTransform;
        private Collider2D _lastCollider;
        private bool _rotationStarted;
        private bool _returnStarted;

        private float _upAngle = 45.0f;
        private float _downAngle = 0.0f;
        private SpinWheelPopup _spinWheelPopup;

        private void Start()
        {
            _spinWheelPopup = GetComponentInParent<SpinWheelPopup>();
            RectTransform.localEulerAngles = new Vector3(0, 0, _downAngle);
        }

        private void LateUpdate()
        {
            return;
            float val = RectTransform.localEulerAngles.z;

            if (_returnStarted)
            {
                val = Mathf.Lerp(val, _downAngle, Time.deltaTime * 10f);
                if (Mathf.Abs(val - _downAngle) < 1f) // Kiểm tra gần đến vị trí gốc
                {
                    val = _downAngle; // Đặt giá trị về vị trí gốc
                    _returnStarted = false; // Đặt lại trạng thái quay về
                }
            }
            else if (_rotationStarted)
            {
                val = Mathf.Lerp(val, _upAngle, Time.deltaTime * 14f);
                if (Mathf.Abs(val - _upAngle) < 1f) // Kiểm tra gần đến góc lên
                {
                    val = _upAngle; // Đặt giá trị về góc lên
                    _rotationStarted = false; // Đặt lại trạng thái quay
                    _Return(); // Bắt đầu quay về
                }
            }

            // Chỉ cập nhật góc khi trạng thái quay hoặc quay về
            if (_returnStarted || _rotationStarted || _spinWheelPopup.IsEnded())
            {
                Vector3 eulerAngles = RectTransform.localEulerAngles;
                eulerAngles.z = val;
                RectTransform.localEulerAngles = eulerAngles;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == _lastCollider)
                return;

            _StartRotation();
            _lastCollider = collision;
        }

        private void _StartRotation()
        { 
            if (!_rotationStarted) // Chỉ bắt đầu quay nếu chưa bắt đầu
            {
                _returnStarted = false;
                _rotationStarted = true;
                _spinWheelPopup.OnTriggerNeedle();
            }
        }

        private void _Return()
        {
            _returnStarted = true;
        }
    }
}