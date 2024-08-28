using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public int ID;
    public GameObject GameObject { get; private set; }
    public bool IsActive { get; private set; }

    public Entity(int id, GameObject gameObject)
    {
        ID = id;
        GameObject = gameObject;
        IsActive = true;
    }

    public void Show()
    {
        GameObject.SetActive(true);
        IsActive = true;
    }

    public void Hide()
    {
        GameObject.SetActive(false);
        IsActive = false;
    }

    public void AttachTo(Transform parent)
    {
        GameObject.transform.SetParent(parent);
    }

    public void Detach()
    {
        GameObject.transform.SetParent(null);
    }
}