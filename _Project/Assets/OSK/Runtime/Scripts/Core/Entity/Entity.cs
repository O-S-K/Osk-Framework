using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : IEntity
{
    public int ID { get; private set; }
    public GameObject gameObject { get; }
    public bool IsActive { get; private set; }

    public Entity(int id, GameObject gameObject)
    {
        ID = id;
        this.gameObject = gameObject;
        IsActive = true;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        IsActive = true;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        IsActive = false;
    }

    public void SetParent(Transform parent)
    {
        this.gameObject.transform.SetParent(parent);
    }

    public void SetParrentNull()
    {
        this.gameObject.transform.SetParent(null);
    }

}