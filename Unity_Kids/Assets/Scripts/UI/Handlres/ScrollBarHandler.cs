using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public sealed class ScrollBarHandler
    {
        private RectTransform contentParent;

        private ScrollRect scrollRect;

        public bool IsInitilized { get; private set; }

        public void Initialize(RectTransform contentParent, ScrollRect scrollRect)
        {
            this.contentParent = contentParent;
            this.scrollRect = scrollRect;

            IsInitilized = true;
        }

        public void FillQuadButtons(QuadSocketObject[] quadSockets)
        {
            foreach (var quadSocket in quadSockets)
            {
                quadSocket.transform.SetParent(contentParent, false);
                quadSocket.gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            scrollRect.horizontal = false;
        }

        public void Activate()
        {
            scrollRect.horizontal = true;
        }
    }
}