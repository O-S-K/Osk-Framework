using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : GameFrameworkComponent
{
    private static Queue<Entity> inactiveEntities = new Queue<Entity>();
    private static List<Entity> activeEntities = new List<Entity>();

    public static Entity Create(Entity entity, int ID)
    {
        if (inactiveEntities.Count > 0)
        {
            Entity _entity = inactiveEntities.Dequeue();
            _entity.Show();
            activeEntities.Add(_entity);
            return _entity;
        }
        else
        {
            Entity newEntity = CreateNew(entity, ID);
            activeEntities.Add(newEntity);
            return newEntity;
        }
    }

    public static Entity Get(Entity entity)
    {
        foreach (var e in activeEntities)
        {
            if (e.ID == entity.ID)
            {
                return e;
            }
        }

        return null;
    }

    private static Entity CreateNew(Entity entity, int id)
    {
        var entityGameObject = Instantiate(entity.GameObject);
        Entity _entity = new Entity(id, entityGameObject);
        return _entity;
    }

    public static void Release(Entity entity)
    {
        if (activeEntities.Contains(entity))
        {
            entity.Hide();
            activeEntities.Remove(entity);
            AddEntityToPool(entity);
        }
        else
        {
            Debug.LogError("Entity not managed by this manager.");
        }
    }

    private static void AddEntityToPool(Entity entity)
    {
        inactiveEntities.Enqueue(entity);
    }

    public static void DestroyAllEntities()
    {
        foreach (Entity entity in activeEntities)
        {
            Destroy(entity.GameObject);
        }

        activeEntities.Clear();
        inactiveEntities.Clear();
    }
}