using R3;
using UnityEngine;

namespace Boot
{
    public class SceneBoot : MonoBehaviour
    {
        public void Boot(out Subject<SceneExitParams> onExit)
        {
            var exitSubject = new Subject<SceneExitParams>();
            
            onExit = exitSubject;
        }
    }
}