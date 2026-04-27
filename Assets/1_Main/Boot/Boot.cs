using System.Collections;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            G.Register(new Data.Provider());
            
            // Remove temporal editor code
            #if UNITY_EDITOR
            
            Debug.LogWarning("Remove temporal editor code (Switch to active scene)");
            var sceneName = SceneManager.GetActiveScene().name;
            
            switch (sceneName)
            {
                case nameof(Scenes.SceneNames.Menu):
                    G.Resolve<Coroutines>().Start(LoadMenu(), out _);
                    return;
                
                case nameof(Scenes.SceneNames.Game):
                    G.Resolve<Coroutines>().Start(LoadGame(), out _);
                    return;
            }

            if (sceneName != nameof(Scenes.SceneNames.Boot)) return;
            
            #endif
            //
            
            G.Resolve<Coroutines>().Start(LoadMenu(), out _);
        }

        private IEnumerator BeforeEveryLoad()
        {
            yield return G.Resolve<Data.Provider>().LoadData();
            
            
            G.Dispose(G.DisposeMethod.Scoped);
            
            yield return null;
        }
        
        private IEnumerator LoadMenu()
        {
            yield return BeforeEveryLoad();

            var menu = new MenuBoot();
            menu.Boot(out var onExit);
            onExit.Subscribe(x => G.Resolve<Coroutines>().Start(LoadGame(), out _));
        }

        private IEnumerator LoadGame()
        {
            yield return BeforeEveryLoad();
            
            var game = new GameBoot();
            game.Boot(out var onExit);
            onExit.Subscribe(x => G.Resolve<Coroutines>().Start(LoadMenu(), out _));
        }
    }
}