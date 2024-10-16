using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class EntityManager : GameFrameworkComponent
    {
        [ShowInInspector, ReadOnly] private List<Entity> activeEntities = new List<Entity>();

        public Entity Create(string name)
        {
            var entity = new Entity();
            entity.gameObject = new GameObject("E." + name);
            activeEntities.Add(entity);
            return entity;
        }

        public Entity Create(IEntity entity, int id)
        {
            if (activeEntities.Any(e => e.ID == id))
            {
                OSK.Logg.LogError("Entity with ID " + id + " already exists");
                return null;
            }

            var newEntity = new Entity();
            newEntity.ID = id;
            newEntity.gameObject = Instantiate(entity.gameObject);
            activeEntities.Add(newEntity);
            return newEntity;
        }

        public T Create<T>(string name, Action onCreate = null) where T : Component
        {
            var entity = new Entity();
            entity.gameObject = new GameObject("E." + name);
            entity.gameObject.AddComponent<T>();
            activeEntities.Add(entity);
            onCreate?.Invoke();
            return entity.gameObject.GetComponent<T>();
        }

        public Entity Get(int id)
        {
            return activeEntities.FirstOrDefault(e => e.ID == id);
        }

        public Entity Get<T>() where T : Component
        {
            return activeEntities.FirstOrDefault(e => e.gameObject.GetComponent<T>() != null);
        }

        public T Get<T>(string name) where T : Component
        {
            return activeEntities.FirstOrDefault(e => e.gameObject.GetComponent<T>() != null).gameObject
                .GetComponent<T>();
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
            if (entity != null)
            {
                Destroy(entity.gameObject);
                activeEntities.Remove(entity);
            }
        }

        public void Destroy(Entity entity)
        {
            if (activeEntities.Contains(entity))
            {
                Destroy(entity.gameObject);
                activeEntities.Remove(entity);
            }
        }

        public void Remove(Entity entity)
        {
            if (activeEntities.Contains(entity))
            {
                activeEntities.Remove(entity);
            }
        } 
    }
}