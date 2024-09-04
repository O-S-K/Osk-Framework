using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;

public class EntityManager : GameFrameworkComponent
{
    [ShowInInspector, SerializeField] 
    private List<Entity> activeEntities = new List<Entity>();

    public Entity Create(string name)
    {
        var entity = new Entity();
        entity.gameObject =  new GameObject("E." + name);
        activeEntities.Add(entity);
        return entity;
    }  
    
    public Entity Create(IEntity entity, int id)
    {
        if(activeEntities.Any(e => e.ID == id))
        {
            Debug.LogError("Entity with ID " + id + " already exists");
            return null;
        }
        
        var newEntity = new Entity();
        newEntity.ID = id;
        newEntity.gameObject = Instantiate(entity.gameObject);
        activeEntities.Add(newEntity);
        return newEntity;
    }
    
    public Entity Get(int id)
    {
        return activeEntities.FirstOrDefault(e => e.ID == id);
    }
    
    public Entity Get<T>() where T : Component
    {
        return activeEntities.FirstOrDefault(e => e.gameObject.GetComponent<T>() != null);
    }
    
    public Entity GetByID(int id)
    {
        return activeEntities.FirstOrDefault(e => e.ID == id);
    }
    
    public List<Entity> GetAll()
    {
        return activeEntities;
    } 
    
    public void Destroy(int id)
    {
        var entity = activeEntities.FirstOrDefault(e => e.ID == id);
        if(entity != null)
        {
            Destroy(entity.gameObject);
            activeEntities.Remove(entity);
        }
    }
    
    public void Destroy(Entity entity)
    {
        if(activeEntities.Contains(entity))
        {
            Destroy(entity.gameObject);
            activeEntities.Remove(entity);
        } 
    }
    
    public void Remove(Entity entity)
    {
        activeEntities.Remove(entity);
    }
}