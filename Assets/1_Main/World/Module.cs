using Config;
using NaughtyAttributes;
using UnityEngine;

namespace World
{
    public class Module : StageObject
    {
        [ReadOnly] public string key;
        [ReadOnly] public Vector3 position;
        [ReadOnly] public Vector3 rotation;

        protected override void OnApplyChanges()
        {
            key = gameObject.name;
            position = transform.position;
            rotation = transform.eulerAngles;
        }

        public override ConfigBase<T> ToConfig<T>()
        {
            if (typeof(T) != typeof(Data.State.Module)) return null;
            return new ModuleConfig(key, position, rotation) as ConfigBase<T>;
        }
    }
}