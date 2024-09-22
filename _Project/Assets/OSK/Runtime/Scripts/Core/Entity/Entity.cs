using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : IEntity
{
    public int ID { get; set; }
    public GameObject gameObject { get; set; }
    public bool IsActive { get; set; }
    
    public List<EComponent> components = new List<EComponent>();

    public Entity()
    {
    }
    

    public T Add<T>() where T : EComponent
    {
        if(components.Any(c => c.GetType() == typeof(T)))
        {
            Debug.LogError("Component " + typeof(T) + " already exists");
            return default;
        }
        var component = gameObject.AddComponent<T>();
        components.Add(component);
        return component as T;
    }

    public T Get<T>() where T : EComponent
    {
        return (T)components.FirstOrDefault(c => c.GetType() == typeof(T));
    }
    
    public virtual void OnTick()
    {
    }


    public void Show()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void SetParent(Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }

    public void SetParentNull()
    {
        gameObject.transform.SetParent(null);
    }
}