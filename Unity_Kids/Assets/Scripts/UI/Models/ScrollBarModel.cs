using UnityEngine;
using UnityEngine.UI;
using Controller;

namespace Models
{
    public sealed class ScrollBarModel : MonoBehaviour
    {
        [SerializeField]
        private RectTransform contentParent;

        [SerializeField]
        private ScrollRect scrollRect;

        private ScrollBarController scrollBarHandler;

        public ScrollBarController GetHandler()
        {
            if (scrollBarHandler == null || !scrollBarHandler.IsInitilized)
            {
                scrollBarHandler = new();
                scrollBarHandler.Initialize(contentParent, scrollRect);
            }

            return scrollBarHandler;
        }
    }
}