using System.Collections.Generic;
using System.Linq;
using Config;
using NaughtyAttributes;
using UnityEngine;

namespace World
{
    [AddComponentMenu("1 World/Entity")]
    public class Entity : StageObject
    {
        [ReadOnly] public string key;
        [ReadOnly] public int priority;
        [ReadOnly] public Vector3 position;
        [ReadOnly] public Vector3 rotation;
        [ReadOnly] public List<Module> modules;
        
        protected override void OnApplyChanges()
        {
            key = gameObject.name;
            position = transform.position;
            rotation = transform.eulerAngles;
            modules = transform.GetComponentsInChildren<Module>().ToList();
            foreach (var module in modules) module.ApplyChanges();
            if (transform.parent != null) priority = transform.GetSiblingIndex();
        }

        public override ConfigBase<T> ToConfig<T>()
        {
            if (typeof(T) != typeof(Data.State.Entity)) return null;
            var confModules = modules.Select(m => (ModuleConfig)m.ToConfig<Data.State.Module>()).ToList();
            var entityConf = new EntityConfig(key, priority, position, rotation, confModules);
            return entityConf as ConfigBase<T>;
        }
    }
}
