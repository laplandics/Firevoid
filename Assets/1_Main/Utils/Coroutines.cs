using System.Collections;
using UnityEngine;

namespace Utils
{
    public class Coroutines
    {
        internal class CoroutineHolder : MonoBehaviour { }
        private readonly CoroutineHolder _coroutineHolder;

        public Coroutines()
        {
            _coroutineHolder = new GameObject("[COROUTINES]").AddComponent<CoroutineHolder>();
            Object.DontDestroyOnLoad(_coroutineHolder.gameObject);
        }
        
        public void Start(IEnumerator enumerator, out Coroutine coroutine,
            MonoBehaviour coroutineHolder = null)
        {
            if (coroutineHolder == null) coroutineHolder = _coroutineHolder;
            coroutine = null; if (enumerator == null) return;
            coroutine = coroutineHolder.StartCoroutine(enumerator);
        }

        public void Stop(Coroutine coroutine, MonoBehaviour coroutineHolder = null)
        {
            if (coroutine == null) return;
            if (coroutineHolder == null) coroutineHolder = _coroutineHolder;
            coroutineHolder.StopCoroutine(coroutine);
        }
    }
}