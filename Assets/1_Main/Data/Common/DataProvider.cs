using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data.Config;
using Newtonsoft.Json;
using Tools;
using UnityEngine;

namespace Data
{
    public class DataProvider
    {
        private readonly Dictionary<string, DataProxy> _proxiesMap = new();
        
        public DataProvider() => JsonSettingsSetter.SetSettings();
        
        private string GetPath(string tag)
        {
            const string folder = "Saves";
            var app = Application.persistentDataPath;
            var directory = Path.Combine(app, folder);
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, $"{tag}.json");
            return path;
        }

        public IEnumerator LoadStageData(string tag)
        {
            var path = GetPath(tag);
            
            State.Stage state;
            if (File.Exists(path))
            {
                var task = File.ReadAllTextAsync(path);
                yield return new WaitUntil(() => task.IsCompleted);
                state = JsonConvert.DeserializeObject<State.Stage>(task.Result);
            }
            else state = ConfigDeserializer.DeserializeStage(tag);
            
            var proxy = new Proxy.Stage(state);
            _proxiesMap[tag] = proxy;
        }

        public IEnumerator SaveData(string tag)
        {
            var proxy = _proxiesMap[tag];
            var state = proxy.GetState;
            var path = GetPath(tag);

            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            var task = File.WriteAllTextAsync(path, json);
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public T GetProxy<T>(string tag) where T : DataProxy
        {
            if (!_proxiesMap.TryGetValue(tag, out var proxy)) return null;
            if (proxy is not T requiredProxy) return null;
            return requiredProxy;
        }
    }
}