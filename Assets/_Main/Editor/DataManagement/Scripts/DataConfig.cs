using System.IO;
using Data;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace EditorTools.DataManagement
{
    public abstract class DataConfig : ScriptableObject
    {
        private const string JSON_FOLDER_PATH = "Assets/_Main/Resources/Json/";
        protected abstract string FileName { get; }
        
        [Button]
        private void ConvertToJson()
        {
            Tools.JsonSettingsSetter.SetSettings();
            
            var state = ToState();
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            var path = JSON_FOLDER_PATH + FileName + ".json";
            File.WriteAllText(path, json);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        protected abstract DataState ToState();
    }
}