using UnityEngine;
using UnityEngine.EventSystems;

namespace Space
{
    public class UIRoot : MonoBehaviour
    {
        public Canvas canvas;
        public EventSystem eventSystem;
        [Space(10)]
        public RectTransform canvasTransform;
        [Space(10)]
        public RectTransform screenContainer;
        public RectTransform windowsContainer;
        public RectTransform tokensContainer;
    }
}