using System.Collections;
using Constant;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Scenes
    {
        public IEnumerator ToBoot()
        {
            yield return SceneManager.LoadSceneAsync("Boot");
            yield return null;
        }

        public IEnumerator ToScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }
}