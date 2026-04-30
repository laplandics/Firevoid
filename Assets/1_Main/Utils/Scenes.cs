using System.Collections;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Scenes
    {
        public IEnumerator Load(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync("Boot");
            yield return null;
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }
}