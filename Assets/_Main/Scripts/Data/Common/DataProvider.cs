using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Tools;
using UnityEngine;

namespace Data
{
    public class DataProvider
    {
        private readonly Dictionary<string, DataProxy> _proxiesMap = new();
        
        public DataProvider() => JsonSettingsSetter.SetSettings();
        
        private string GetPath(string label)
        {
            const string folder = "Saves";
            var app = Application.persistentDataPath;
            var directory = Path.Combine(app, folder);
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, $"{label}.json");
            return path;
        }

        public IEnumerator LoadData<T>(string label) where T : DataState
        {
            var path = GetPath(label);

            DataState state;
            if (File.Exists(path))
            {
                var task = File.ReadAllTextAsync(path);
                yield return new WaitUntil(() => task.IsCompleted);
                state = JsonConvert.DeserializeObject<T>(task.Result);
            }
            else
            {
                var request = Resources.LoadAsync<TextAsset>($"Json/{label}");
                yield return new WaitUntil(() => request.isDone);
                var json = request.asset as TextAsset;
                if (json == null) throw new Exception($"Failed to find json asset {label}");
                state = JsonConvert.DeserializeObject<T>(json.text);
            }
            
            var stateTypeName = state.GetType().Name;
            var proxyType = Type.GetType($"Data.Proxy.{stateTypeName}, Assembly-CSharp");
            if (proxyType == null) throw new Exception($"Failed to find proxy 'Data.Proxy.{stateTypeName}'");
            var proxy = (DataProxy)Activator.CreateInstance(proxyType, state);
            
            _proxiesMap[label] = proxy;
        }
        
        public IEnumerator SaveData(string label)
        {
            var proxy = _proxiesMap[label];
            var state = proxy.Origin;
            var path = GetPath(label);

            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            var task = File.WriteAllTextAsync(path, json);
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public T GetProxy<T>(string label) where T : DataProxy
        {
            if (!_proxiesMap.TryGetValue(label, out var proxy)) return null;
            if (proxy is not T requiredProxy) return null;
            return requiredProxy;
        }
    }
}