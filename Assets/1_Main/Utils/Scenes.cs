using System.Collections;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Scenes
    {
        public enum SceneNames { Boot, Game, Menu }
        
        public IEnumerator Load(SceneNames sceneName)
        {
            yield return SceneManager.LoadSceneAsync("Boot");
            yield return null;
            yield return SceneManager.LoadSceneAsync(sceneName.ToString());
            yield return null;
        }
    }
}