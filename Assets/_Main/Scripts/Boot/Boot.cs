using System;
using System.Collections;
using Constant;
using Data;
using Parameters;
using R3;
using Space;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Object = UnityEngine.Object;

namespace Boot
{
    public class ProjectBoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Bootstrap() => _ = new ProjectBoot();

        private ProjectBoot()
        {
            G.Dispose(G.DisposeMethod.Full);
            
            G.Register(new UI());
            G.Register(new Input());
            G.Register(new Scenes());
            G.Register(new Services());
            G.Register(new Entities());
            G.Register(new Coroutines());
            G.Register(new DataProvider());

            G.Resolve<Services>().OnProjectBeginLoad();
            
            var sceneName = SceneManager.GetActiveScene().name;
            if (!Enum.TryParse<StageNames>(sceneName, out _) && 
                sceneName != "Menu" && sceneName != "Boot") return;
            
            G.Resolve<Services>().OnProjectEndLoad();
            
            G.Resolve<Coroutines>().Start(BeforeFirstLoad(), out _);
        }

        private IEnumerator BeforeFirstLoad()
        {
            yield return G.Resolve<Scenes>().ToBoot();
            yield return G.Resolve<Services>().OnProjectBeginBoot();
            yield return null;
            
            const string settingsTag = nameof(Data.State.App);
            yield return G.Resolve<DataProvider>().LoadData<Data.State.App>(settingsTag);
            var appSettings = G.Resolve<DataProvider>().GetProxy<Data.Proxy.App>(settingsTag);
            Application.targetFrameRate = appSettings.FPS.Value;
            QualitySettings.vSyncCount = appSettings.VSync.Value;
            yield return null;

            yield return G.Resolve<Services>().OnProjectEndBoot();
            yield return null;

            var firstSceneParams = appSettings.FirstSceneParams;
            G.Resolve<Coroutines>().Start(LoadScene(firstSceneParams), out _);
        }
        
        private IEnumerator LoadScene(SceneParams loadParams)
        {
            yield return new WaitForSeconds(2f);
            
            yield return G.Resolve<Scenes>().ToScene(loadParams.sceneName);
            yield return G.Resolve<Services>().OnSceneBeginLoad();
            
            G.Register(new World());
            yield return null;
            
            var sceneBoot = Object.FindAnyObjectByType<SceneBoot>();
            if (sceneBoot == null) 
            {
                Debug.LogWarning($"Couldn't find SceneBoot on this scene ({loadParams.sceneName})");
                yield break;
            }
            
            var onExit = new Subject<SceneParams>();
            onExit.Subscribe(sceneParams => G.Resolve<Coroutines>().Start(UnloadScene(sceneParams), out _));
            
            yield return G.Resolve<Services>().OnSceneEndLoad();
            yield return null;
            
            G.Resolve<Coroutines>().Start(sceneBoot.Boot(onExit), out _);
        }

        private IEnumerator UnloadScene(SceneParams unloadParams)
        {
            yield return G.Resolve<Services>().OnSceneBeginUnload();
            yield return null;
            
            G.Dispose(G.DisposeMethod.Scoped);
            yield return null;
            
            yield return G.Resolve<Services>().OnSceneEndUnload();
            yield return null;
            
            yield return G.Resolve<Scenes>().ToBoot();
            G.Resolve<Coroutines>().Start(LoadScene(unloadParams), out _);
        }
    }
}