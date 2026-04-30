using Config;
using Data;
using NaughtyAttributes;
using UnityEngine;

namespace World
{
    public abstract class StageObject : MonoBehaviour
    {
        [Button]
        public void ApplyChanges() => OnApplyChanges();
        protected virtual void OnApplyChanges() {}
        public abstract ConfigBase<T> ToConfig<T>() where T : DataState;
    }
}