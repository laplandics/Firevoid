using System;
using Data.State;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class ModuleConfig : ConfigBase<Module>
    {
        public string key;
        public Vector3 position;
        public Vector3 rotation;

        public ModuleConfig(string key, Vector3 position, Vector3 rotation)
        { this.key = key; this.position = position; this.rotation = rotation; }
        
        public override Module ToState()
        {
            var moduleState = new Module();
            moduleState.Key = key;
            moduleState.Position = position;
            moduleState.Rotation = rotation;
            return moduleState;
        }
    }
}