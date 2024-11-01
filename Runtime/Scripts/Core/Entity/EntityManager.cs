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
        [ShowInInspector, ReadOnly] private List<Entity> _listEntitiesActive = new List<Entity>();

        public override void OnInit() {}

        public Entity Create(string name)
        {
            var entity = new Entity();
            entity.gameObject = new GameObject("E." + name);
            _listEntitiesActive.Add(entity);
            return entity;
        }

        public Entity Create(IEntity entity, int id)
        {
            if (_listEntitiesActive.Any(e => e.ID == id))
            {
                OSK.Logg.LogError("Entity with ID " + id + " already exists");
                return null;
            }

            var newEntity = new Entity();
            newEntity.ID = id;
            newEntity.gameObject = Instantiate(entity.gameObject);
            _listEntitiesActive.Add(newEntity);
            return newEntity;
        }

        public T Create<T>(string name, Action onCreate = null) where T : Component
        {
            var entity = new Entity();
            entity.gameObject = new GameObject("E." + name);
            entity.gameObject.AddComponent<T>();
            _listEntitiesActive.Add(entity);
            onCreate?.Invoke();
            return entity.gameObject.GetComponent<T>();
        }

        public Entity Get(int id)
        {
            return _listEntitiesActive.FirstOrDefault(e => e.ID == id);
        }

        public Entity Get<T>() where T : Component
        {
            return _listEntitiesActive.FirstOrDefault(e => e.gameObject.GetComponent<T>() != null);
        }

        public T Get<T>(string name) where T : Component
        {
            return _listEntitiesActive.FirstOrDefault(e => e.gameObject.GetComponent<T>() != null).gameObject
                .GetComponent<T>();
        }

        public Entity GetByID(int id)
        {
            return _listEntitiesActive.FirstOrDefault(e => e.ID == id);
        }

        public List<Entity> GetAll()
        {
            return _listEntitiesActive;
        }

        public void Destroy(int id)
        {
            var entity = _listEntitiesActive.FirstOrDefault(e => e.ID == id);
            if (entity != null)
            {
                Destroy(entity.gameObject);
                _listEntitiesActive.Remove(entity);
            }
        }

        public void Destroy(Entity entity)
        {
            if (_listEntitiesActive.Contains(entity))
            {
                Destroy(entity.gameObject);
                _listEntitiesActive.Remove(entity);
            }
        }

        public void Remove(Entity entity)
        {
            if (_listEntitiesActive.Contains(entity))
            {
                _listEntitiesActive.Remove(entity);
            }
        } 
    }
}