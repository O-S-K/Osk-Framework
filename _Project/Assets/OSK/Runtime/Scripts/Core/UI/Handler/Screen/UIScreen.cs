using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;

namespace OSK
{
    public abstract class UIScreen : MonoBehaviour
    {
        public List<ElementUI> elementUIList = new List<ElementUI>();
        protected ElementUI currentElementUI;
        public bool IsShowing => _isShowing;
        [ReadOnly, ShowInInspector] private bool _isShowing;

        public abstract void Initialize();

        public virtual void Show()
        {
            _isShowing = true;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            _isShowing = false;
            gameObject.SetActive(false);
        }

        public virtual void RefreshUI()
        {
        }

        public void AddElementToList(ElementUI element)
        {
            if (!elementUIList.Contains(element))
            {
                elementUIList.Add(element);
            }
        }

        public void RemoveElementToList(ElementUI element)
        {
            elementUIList.Remove(element);
        }


        public T ShowElement<T>() where T : ElementUI
        {
            foreach (ElementUI elementUI in elementUIList)
            {
                if (elementUI is T)
                {
                    elementUI.Show();
                    currentElementUI = elementUI;
                    return currentElementUI as T;
                }
            }

            return null;
        }

        public T GetElement<T>() where T : ElementUI
        {
            foreach (ElementUI elementUI in elementUIList)
            {
                if (elementUI is T)
                {
                    return elementUI as T;
                }
            }

            Debug.LogError($"[ElementUIList] No element exists in List");
            return null;
        }
    }
}