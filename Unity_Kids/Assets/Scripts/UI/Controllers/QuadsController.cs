using UnityEngine;
using Objects;
using System;
using Utils;

namespace Controllers
{
    public sealed class QuadsController : IDisposable
    {
        private Transform releasedQuadsParent;
        private RectTransform playZone;
        private RectTransform canvasRectTransform;
        private GarbageCollectorObject garbageCollectorObject;
        private TowerHead towerHead;

        private QuadsBuildController quadsBuilder;

        private QuadObject currentQuadObject;

        private bool isFirstBlock = true;

        public event Action QuadBuilded;
        public event Action QuadDestroyed;
        public event Action OutsidePlayingAreaWent;

        public QuadsController(
            Transform releasedQuadsParent,
            RectTransform playZone,
            TowerHead towerHead,
            RectTransform canvasRectTransform,
            GarbageCollectorObject garbageCollectorObject)
        {
            this.releasedQuadsParent = releasedQuadsParent;
            this.playZone = playZone;
            this.canvasRectTransform = canvasRectTransform;
            this.garbageCollectorObject = garbageCollectorObject;
            this.towerHead = towerHead;

            quadsBuilder = new(releasedQuadsParent, towerHead);

            quadsBuilder.TowerEmpty += TowerEmpty;
            quadsBuilder.MoveQuadTo += MoveQuadTo;
            quadsBuilder.JumpQuadTo += JumpQuadTo;
        }

        public void SetCurrentQuad(QuadObject quad)
        {
            currentQuadObject = quad;
            currentQuadObject.BeginDragged -= SetCurrentQuad;
            currentQuadObject.EndDragged -= OnQuadEndDragged;

            currentQuadObject.transform.SetParent(releasedQuadsParent);

            currentQuadObject.BeginDragged += SetCurrentQuad;
            currentQuadObject.EndDragged += OnQuadEndDragged;
        }

        private void OnQuadEndDragged()
        {
            if (currentQuadObject.IsTowerPart)
            {
                TowerPartHandler();
                return;
            }

            if(!UIObjectInsideCheck.IsFullyInside(playZone, currentQuadObject.RectTransform)
                || !UIObjectInsideCheck.IsFullyInside(canvasRectTransform, currentQuadObject.RectTransform))
            {
                currentQuadObject.Destroyed += OnQuadDestroyed;

                currentQuadObject.DestroyAnimation();
                currentQuadObject = null;

                OutsidePlayingAreaWent?.Invoke();
                return;
            }

            FreeQuadHandler();
        }

        private void TowerPartHandler()
        {
            if (UIObjectInsideCheck.IsInside(garbageCollectorObject.RectTransform, currentQuadObject.RectTransform))
            {
                currentQuadObject.Destroyed += OnQuadDestroyed;

                currentQuadObject.MoveTo(garbageCollectorObject.RectTransform.position);
                currentQuadObject.DestroyAnimation();

                QuadDestroyed.Invoke();

                quadsBuilder.DeleteQuad(currentQuadObject);
                return;
            }

            var targetPosition = quadsBuilder.GetQuadTowerPosition(currentQuadObject);
            MoveQuadTo(currentQuadObject, targetPosition);
        }

        private void MoveQuadTo(QuadObject quadObject, Vector3 targetPoint)
        {
            quadObject.MoveTo(targetPoint);
        }

        private void JumpQuadTo(QuadObject quadObject, Vector3 targetPoint)
        {
            quadObject.JumpAnimation(targetPoint);
        }

        public void TowerEmpty()
        {
            isFirstBlock = true;
        }

        private void FreeQuadHandler()
        {
            if (isFirstBlock)
            {
                isFirstBlock = false;

                quadsBuilder.SetFirstQuad(currentQuadObject);
                QuadBuilded?.Invoke();
                return;
            }

            if (UIObjectInsideCheck.IsInside(towerHead.RectTransform, currentQuadObject.RectTransform)
                && towerHead.RectTransform.position.y < currentQuadObject.RectTransform.position.y)
            {
                quadsBuilder.SetQuad(currentQuadObject);
                QuadBuilded?.Invoke();
                return;
            }

            currentQuadObject.Destroyed += OnQuadDestroyed;

            QuadDestroyed.Invoke();
            currentQuadObject.DestroyAnimation();
            currentQuadObject = null;
        }

        private void OnQuadDestroyed(QuadObject quad)
        {
            quad.Destroyed -= OnQuadDestroyed;

            GameObject.Destroy(quad.gameObject);
        }

        public void Dispose()
        {
            quadsBuilder.TowerEmpty -= TowerEmpty;
            quadsBuilder.MoveQuadTo -= MoveQuadTo;
            quadsBuilder.JumpQuadTo -= JumpQuadTo;
        }
    }
}