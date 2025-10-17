using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Objects
{
    public sealed class QuadObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private Image quadImage;

        public int QuadId { get; private set; }

        public event Action<QuadObject> Release;

        public new event Action<QuadObject> Destroy;

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

        public void SpawnAnimation()
        {
            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutBack);
        }

        public void DestroyAnimation()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBack)
                .OnComplete(() => {
                    Destroy?.Invoke(this);
                });
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Release?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}