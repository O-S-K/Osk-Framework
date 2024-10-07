using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
public class ElementUI : MonoBehaviour
{
    protected void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
    }

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
}

}