using Data;
using Data.State;
using Parameters;
using UnityEngine;

namespace EditorTools.DataManagement
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "Editor/DataConfig/AppSettings")]
    public class AppDC : DataConfig
    {
        public int fps;
        public int vSync;
        [Space]
        public SceneParams firstSceneParams;
        
        protected override string FileName => nameof(App);

        protected override DataState ToState()
        {
            var appSettingsState = new App();
            appSettingsState.FPS = fps;
            appSettingsState.VSync = vSync;
            appSettingsState.FirstSceneParams = firstSceneParams;
            return appSettingsState;
        }
    }
}