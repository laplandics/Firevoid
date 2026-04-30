using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data.State;

namespace Config
{
    [Serializable]
    public class EntityConfig : ConfigBase<Entity>
    {
        public string key;
        public int priority;
        public Vector3 position;
        public Vector3 rotation;
        public List<ModuleConfig> modules;

        public EntityConfig
        (
            string key,
            int priority,
            Vector3 position,
            Vector3 rotation,
            List<ModuleConfig> modules
        )
        {
            this.key = key;
            this.priority = priority;
            this.position = position;
            this.rotation = rotation;
            this.modules = modules;
        }
        
        public override Entity ToState()
        {
            var entityState = new Entity();
            entityState.ID = Guid.NewGuid().ToString();
            entityState.Key = key;
            entityState.Priority = priority;
            entityState.Position = position;
            entityState.Rotation = rotation;
            entityState.Modules = modules.Select(module => module.ToState()).ToList();
            return entityState;
        }
    }
}