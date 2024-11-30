using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    public class EntityManager : GameFrameworkComponent
    {
        [ShowInInspector, ReadOnly] private List<Entity> _listEntitiesActive = new List<Entity>();

        public override void OnInit() {}

        public Entity Create(string name)
        {
            var existingEntity = _listEntitiesActive.FirstOrDefault(e => e.name == name);
            if (existingEntity != null)
            {
                return existingEntity;
            }

            var entity = new GameObject(name).AddComponent<Entity>();
            _listEntitiesActive.Add(entity);
            return entity;
        }

        public Entity Create(IEntity entity, int id)
        {
            var existingEntity = _listEntitiesActive.FirstOrDefault(e => e.ID == id);
            if (existingEntity != null)
            {
                return existingEntity;
            }

            var newEntity = entity as Entity;
            if (newEntity != null)
            {
                newEntity.ID = id;
                _listEntitiesActive.Add(newEntity);
                return newEntity;
            }
            return null;
        }

        public Entity Create<T>(string name) where T : Component
        {
            var existingEntity = _listEntitiesActive.FirstOrDefault(e => e.name == name);
            if (existingEntity != null)
            {
                return existingEntity;
            }

            var entity = new GameObject(name).gameObject.AddComponent<Entity>();
            _listEntitiesActive.Add(entity);
            return entity;
        }
        
        public bool Has(int id)
        {
            return _listEntitiesActive.Any(e => e.ID == id);
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