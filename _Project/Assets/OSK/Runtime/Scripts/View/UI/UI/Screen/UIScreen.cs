using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public abstract class UIScreen : MonoBehaviour
    {
        public List<ElementUI> elementUIList = new List<ElementUI>();
        protected ElementUI currentElementUI;

        public abstract void Initialize();
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
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
                if(elementUI is T)
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
            foreach(ElementUI elementUI in elementUIList)
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
