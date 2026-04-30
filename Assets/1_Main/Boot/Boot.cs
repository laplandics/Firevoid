using System.Collections;
using Data;
using R3;
using UnityEngine;
using Utils;

namespace Boot
{
    public class ProjectBoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap() => _ = new ProjectBoot();

        private ProjectBoot()
        {
            G.Register(new Coroutines());
            G.Register(new Scenes());
            G.Register(new Input());
            
            G.Register(new DataProvider());
            
            G.Resolve<Coroutines>().Start(LoadScene(Const.MENU_SCENE_NAME), out _);
        }
        
        private IEnumerator LoadScene(string sceneName)
        {
            G.Dispose(G.DisposeMethod.Scoped);
            yield return new WaitForSeconds(2f);
            
            yield return G.Resolve<DataProvider>().LoadStageData(sceneName);
            
            yield return G.Resolve<Scenes>().Load(sceneName);
            yield return null;
            
            var sceneBoot = Object.FindAnyObjectByType<SceneBoot>();
            sceneBoot.Boot(out var onExit);
            onExit.Subscribe(exitParams => G.Resolve<Coroutines>().Start(LoadScene(exitParams.NextSceneName), out _));
        }
    }
}