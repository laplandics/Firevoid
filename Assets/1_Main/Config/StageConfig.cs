using System;
using System.Collections.Generic;
using System.Linq;
using Data.State;
using NaughtyAttributes;

namespace Config
{
    [Serializable]
    public class StageConfig : ConfigBase<Stage>
    {
        [ReadOnly] public List<SpawnerConfig> spawners;
        [ReadOnly] public List<EntityConfig> entities;

        public StageConfig(List<SpawnerConfig> spawners, List<EntityConfig> entities)
        { this.spawners = spawners; this.entities = entities; }
        
        public override Stage ToState()
        {
            var stageState = new Stage();
            stageState.Spawners = spawners.Select(spawner => spawner.ToState()).ToList();
            stageState.Entities = entities.Select(entity => entity.ToState()).ToList();
            return stageState;
        }
    }
}