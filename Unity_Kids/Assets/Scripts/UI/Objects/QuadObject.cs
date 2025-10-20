using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Objects
{
    public sealed class QuadObject : PoolableObject, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField]
        public RectTransform RectTransform { get; private set; }

        [SerializeField]
        private Image quadImage;

        public event Action<QuadObject> BeginDragged;
        public event Action<QuadObject> Destroyed;
        public event Action EndDragged;
        public event Action JumpAnimationEnd;

        private Canvas canvas;

        public bool IsTowerPart { get; private set; }

        public int QuadId { get; private set; }

        public bool isInSocket = true;

        public void Initialize(int quadId, Canvas canvas)
        {
            QuadId = quadId;
            this.canvas = canvas;

            isInSocket = true;
            IsTowerPart = false;

            transform.localScale = Vector3.one;
        }

        public void DeleteFromSocket()
        {
            isInSocket = false;

            GoToIdleSize();
        }

        public void SetAsTowerPart()
        {
            isInSocket = false;
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

        public void DestroyWithAnimation()
        {
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBack)
                .OnComplete(() => {
                    Destroyed?.Invoke(this);
                });
        }

        public void JumpWithAnimation(Vector3 targetValue)
        {
            transform.DOJump(targetValue, 0.5f, 1, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                JumpAnimationEnd?.Invoke();
            });
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragged?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragged?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isInSocket)
            {
                return;
            }

            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f).SetEase(Ease.InOutCubic);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isInSocket)
            {
                return;
            }

            GoToIdleSize();
        }

        private void GoToIdleSize()
        {
            transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutCubic);
        }
    }
}