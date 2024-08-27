using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace OSK
{
    public class HoldButtonInTime : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IPointerUpHandler
    {
        public enum ETypeHold
        {
            Button,
            Keyboard
        }

        public ETypeHold TypeHold = ETypeHold.Keyboard;

        public Vector3 hiddenSize = Vector3.zero;
        public float speed = 0.3f;
        public Transform bar;

        public float timeCompleted = 1;
        public UnityEvent Completed;

        private float escHeldFor;
        private bool isCompleted;

        private bool isProgress;

        private void Start()
        {
            transform.localScale = hiddenSize;
            isCompleted = false;
        }

        private void Update()
        {
            if (TypeHold == ETypeHold.Keyboard) // example
            {
                isProgress = Input.GetKey(KeyCode.Escape);
            }

            if (isProgress)
            {
                transform.DOScale(Vector3.one, speed).SetEase(Ease.OutBounce);
            }
            else
            {
                escHeldFor = 0;
            }

            if (isProgress)
            {
                escHeldFor += Time.deltaTime;
                CancelInvoke(nameof(HideText));
                Invoke(nameof(HideText), 2f);
            }

            if (escHeldFor > timeCompleted && !isCompleted)
            {
                isCompleted = true;
                OnCompleted();
            }

            if (bar != null)
            {
                var amount = escHeldFor / timeCompleted;
                bar.transform.localScale = new Vector3(amount, 1f, 1f);
            }
        }

        private void OnCompleted()
        {
            Completed?.Invoke();
        }

        public void HideText()
        {
            //Tweener.Instance.ScaleTo(transform, hiddenSize, speed, 0f, TweenEasings.QuarticEaseIn);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //isProgress = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //isProgress = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            isProgress = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isProgress = false;
        }
    }
}