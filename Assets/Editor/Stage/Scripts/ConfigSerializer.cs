using System.IO;
using Config;
using Newtonsoft.Json;
using Tools;
using UnityEditor;

namespace EditorTools.StageManagement
{
    public static class ConfigSerializer
    {
        public static void SerializeStage(StageConfig stageConfig)
        {
            JsonSettingsSetter.SetSettings();

            const string directory = Const.STAGE_CONFIGS_DIRECTORY;
            var path = $"Assets/Resources/{directory}/{stageConfig.GetType().Name}.json";
            var json = JsonConvert.SerializeObject(stageConfig.ToState(), Formatting.Indented);
            File.WriteAllText(path, json);
            
            AssetDatabase.ImportAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}