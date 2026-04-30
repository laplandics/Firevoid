using System.Linq;
using Config;
using UnityEngine;
using World;
using Entity = Data.State.Entity;
using Spawner = Data.State.Spawner;

namespace EditorTools.StageManagement
{
    public static class SceneToStagePacker
    {
        public static void Pack(StageConfigAsset stageAsset)
        {
            var stageObjects = Object.FindObjectsByType<StageObject>();
            foreach (var stageObject in stageObjects) stageObject.ApplyChanges();
            var spawnerConfigs = stageObjects
                .Select(stageObj => stageObj
                .ToConfig<Spawner>()
                as SpawnerConfig)
                .Where(s => s != null)
                .ToList();
            var entityConfigs = stageObjects
                .Select(stageObj => stageObj
                .ToConfig<Entity>()
                as EntityConfig)
                .Where(e => e != null)
                .ToList();
            var stageConfig = new StageConfig(spawnerConfigs, entityConfigs);
            stageAsset.value = stageConfig;
        }
    }
}
