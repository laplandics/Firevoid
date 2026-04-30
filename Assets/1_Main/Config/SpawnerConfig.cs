using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data.State;

namespace Config
{
    [Serializable]
    public class SpawnerConfig : ConfigBase<Spawner>
    {
        public Vector3 position;
        public List<EntityConfig> entities;

        public SpawnerConfig(Vector3 position, List<EntityConfig> entities)
        { this.position = position; this.entities = entities; }
        
        public override Spawner ToState()
        {
            var spawnerState = new Spawner();
            spawnerState.Position = position;
            spawnerState.Entities = entities.Select(entity => entity.ToState()).ToList();
            return spawnerState;
        }
    }
}