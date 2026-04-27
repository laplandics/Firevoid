using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class Provider
    {
        public Proxy.Project Project { get; private set; }
        
        private static string Path => Application.persistentDataPath + "Proj.json";

        public Provider()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Converters = new List<JsonConverter> { new Tools.Vector3Converter() }
            };
        }

        public IEnumerator LoadData()
        {
            Project = null;
            
            State.Project state;
            if (File.Exists(Path))
            {
                var task = File.ReadAllTextAsync(Path);
                yield return new WaitUntil(() => task.IsCompleted);
                state = JsonConvert.DeserializeObject<State.Project>(task.Result);
            }
            else state = CreateDataState();
            
            Project = new Proxy.Project(state);
            yield return SaveData();
        }

        public IEnumerator SaveData()
        {
            var state = Project.Origin;
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            
            var task = File.WriteAllTextAsync(Path, json);
            yield return new WaitUntil(() => task.IsCompleted);
        }

        private State.Project CreateDataState()
        {
            var state = new State.Project();
            state.Entities = new List<State.Entity>();
            
            return state;
        }
    }
}