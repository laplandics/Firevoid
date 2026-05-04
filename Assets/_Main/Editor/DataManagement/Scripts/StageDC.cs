using System.Collections.Generic;
using Constant;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorTools.DataManagement
{
    [CreateAssetMenu(fileName = "StageInfo", menuName = "Editor/DataConfig/StageInfo")]
    public class StageDC : DataConfig
    {
        protected override string FileName => SceneManager.GetActiveScene().name;
        
        protected override DataState ToState()
        {
            var stageState = new Data.State.Stage();
            stageState.Flags = new List<StageFlags>();
            stageState.Key = FileName;
            return stageState;
        }
    }
}