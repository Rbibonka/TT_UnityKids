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

        [SerializeField]
        private BoxCollider2D collider;

        public RectTransform RectTransform => rectTransform;

        public event Action<QuadObject> Release;
        public new event Action<QuadObject> Destroy;
        public event Action EndDrag;
        public event Action JumpAnimationEnd;

        private Canvas canvas;

        public bool IsTowerPart { get; private set; }

        public int QuadId { get; private set; }

        public void Initialize(int quadId, Canvas canvas)
        {
            QuadId = quadId;
            this.canvas = canvas;
        }

        public void SetAsTowerPart()
        {
            IsTowerPart = true;
        }
        public void SetSprite(Sprite sprite)
        {
            quadImage.sprite = sprite;
        }

        public void MoveTo(Vector3 targetPoint)
        {
            transform.DOMove(targetPoint, 0.2f).SetEase(Ease.InOutSine);
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

        public void JumpAnimation(Vector3 targetValue)
        {
            transform.DOJump(targetValue, 0.5f, 1, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                JumpAnimationEnd?.Invoke();
            });
        }

        public Collider2D[] TakeCollision()
        {
            return Physics2D.OverlapBoxAll(rectTransform.position, Vector3.one, 0f);
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
            EndDrag?.Invoke();
        }
    }
}