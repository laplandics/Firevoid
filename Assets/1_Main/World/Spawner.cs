using System.Collections.Generic;
using System.Linq;
using Config;
using NaughtyAttributes;
using UnityEngine;

namespace World
{
    [AddComponentMenu("1 World/Spawner")]
    public class Spawner : StageObject
    {
        [ReadOnly] public Vector3 position;
        [ReadOnly] public List<Entity> entities;

        protected override void OnApplyChanges()
        {
            position = transform.position;
            entities = GetComponentsInChildren<Entity>().ToList();
            foreach (var entity in entities) entity.ApplyChanges();
        }

        public override ConfigBase<T> ToConfig<T>()
        {
            if (typeof(T) != typeof(Data.State.Spawner)) return null;
            var confEntities = entities.Select(e => (EntityConfig)e.ToConfig<Data.State.Entity>()).ToList();
            var spawnerConfig = new SpawnerConfig(position, confEntities);
            return spawnerConfig as ConfigBase<T>;
        }
    }
}