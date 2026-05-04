using System.Collections;
using Parameters;
using R3;
using UnityEngine;
using Utils;

namespace Boot
{
    public class SceneBoot : MonoBehaviour
    {
        public IEnumerator Boot(Subject<SceneParams> onExit)
        {
            yield return G.Resolve<Services>().OnSceneBeginBoot();
            yield return null;
            
            //Boot

            yield return null;
            yield return G.Resolve<Services>().OnSceneEndBoot();
        }
    }
}