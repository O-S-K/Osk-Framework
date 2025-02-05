using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace OSK
{
    public class SpinWheelPopup : MonoBehaviour
    {
        [System.Serializable]
        public class RewardItem
        {
            public Sprite icon;
            public int count;
        }

        [TableList]
        public RewardItem[] rewards;
        [TableList]
        public RewardItemComponent[] rewardItemComponents;

        public Transform Wheel;
        public AnimationCurve Curve;

        public Collider2D SpinWheelArrowCollider;

        public GameObject ResultPanel;
        public Image ResultIcon;
        public TextMeshProUGUI ResultCount;


        private bool _isStarted;
        private float _startAngle;
        private float _endAngle;
        private int _randomRewardIndex = 0;
        private float _currentRotationTime;
        private float _maxRotationTime;

        private void Start()
        {
            HideResult();
        }

        public void Init()
        {
            ApplyValues();
        }

        public void TurnWheel()
        {
            if (_isStarted)
                return;

            SpinWheelArrowCollider.gameObject.SetActive(true);
            SpinWheelArrowCollider.enabled = true;

            _isStarted = true;
            _startAngle = Wheel.localEulerAngles.z;
            int totalSlots = rewards.Length;
            _randomRewardIndex = Random.Range(0, totalSlots);

            int rotationCount = Random.Range(10, 15);
            _endAngle = -(rotationCount * 360 + _randomRewardIndex * 360 / totalSlots);

            _currentRotationTime = 0.0f;
            _maxRotationTime = Random.Range(5.0f, 9.0f);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TurnWheel();
            }

            if (_isStarted)
            {
                float t = _currentRotationTime / _maxRotationTime;
                t = Curve.Evaluate(t);

                //t = t * t * t * (t * (a * t - b) + c);

                float angle = Mathf.Lerp(_startAngle, _endAngle, t);
                Wheel.eulerAngles = new Vector3(0, 0, angle);

                if (angle <= _endAngle)
                {
                    _isStarted = false;
                    SettleWheel();
                }
                _currentRotationTime += Time.deltaTime;
            }
        }
        
        public bool IsEnded()
        {
            return !_isStarted;
        }

        private void SettleWheel()
        {
            SpinWheelArrowCollider.enabled = false;
            ShowResult(_randomRewardIndex);
        }

        [Button]
        public void ApplyValues()
        {
            int index = 0;
            foreach (var r in rewards)
            {
                if (rewardItemComponents.Length > index)
                {
                    rewardItemComponents[index].SetData(r);
                }
                index++;
            }
        }

        public void ShowResult(int resultIndex)
        {
            _ShowResult(resultIndex).Run();
            Debug.Log("Result: " + resultIndex);
            //SoundController.instance.PlaySoundEffect("spin_win", false, 1);
        }

        private IEnumerator _ShowResult(int resultIndex)
        {
            if (ResultPanel)
            {
                ResultPanel.SetActive(true);
                int actualRewardIndex = rewards.Length - resultIndex; // because wheel rounds in clockwise

                if (rewards.Length > actualRewardIndex)
                {
                    ResultIcon.sprite = rewards[actualRewardIndex].icon;
                    ResultCount.text = "x" + rewards[actualRewardIndex].count.ToString();
                }

                //ResultPanel.GetComponent<Animator>().Play("clip");
            }
            yield return new WaitForSeconds(3.3f);
            HideResult();
        }

        public void HideResult()
        {
            if (ResultPanel)
            {
                ResultPanel.SetActive(false);
            }
        }


        public void OnTriggerNeedle()
        {
            if (_isStarted)
            {
                Debug.Log("Wheel is spinning");
                //SoundController.instance.PlaySoundEffect("spin_tuk", false, 1);
            }
        }

        public void Close()
        {
            RewardSytem.instance.ShowSpinWheelWindow(false);
        }
    }
}
