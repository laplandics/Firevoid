using Config;
using NaughtyAttributes;
using UnityEngine;

namespace EditorTools.StageManagement
{
    public class StageConfigAsset : ScriptableObject
    { [ReadOnly] public StageConfig value; }
}