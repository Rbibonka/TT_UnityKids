using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Objects
{
    public sealed class QuadObject : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private Image quadImage;

        public int QuadId { get; private set; }

        public event Action<int> BeginDrag;

        private Canvas canvas;

        public void Initialize(int quadId, Canvas canvas)
        {
            QuadId = quadId;
            this.canvas = canvas;
        }

        public void SetSprite(Sprite sprite)
        {
            quadImage.sprite = sprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(QuadId);
        }
    }
}