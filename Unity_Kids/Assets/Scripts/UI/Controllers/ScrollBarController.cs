using Objects;
using UnityEngine;

namespace Controller
{
    public sealed class ScrollBarController
    {
        private RectTransform contentParent;

        public bool IsInitilized { get; private set; }

        public ScrollBarController(RectTransform contentParent)
        {
            this.contentParent = contentParent;
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
    }
}