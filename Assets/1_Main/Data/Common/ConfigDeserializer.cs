using Newtonsoft.Json;
using Tools;
using UnityEngine;

namespace Data.Config
{
    public static class ConfigDeserializer
    {
        public static State.Stage DeserializeStage(string tag)
        {
            JsonSettingsSetter.SetSettings();
            
            const string directory = Const.STAGE_CONFIGS_DIRECTORY;
            var path = $"{directory}/{tag}";
            var json = Resources.Load<TextAsset>(path);
            var state = JsonConvert.DeserializeObject<State.Stage>(json.text);
            return state;
        }
    }
}