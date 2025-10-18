using UnityEngine;
using Objects;
using Configs;
using System;

namespace Handlers
{
    public sealed class QuadsHanlder : IDisposable
    {
        private QuadConfig[] quads;
        private Canvas canvas;
        private QuadObject quadObject;
        private QuadSocketObject quadSocketObject;
        private Transform releasedQuadsParent;
        private RectTransform playZone;
        private RectTransform canvasRectTransform;
        private GarbageCollectorObject garbageCollectorObject;
        private TowerHead towerHead;

        private QuadsBuilder quadsBuilder;

        private QuadObject currentQuadObject;

        private bool isFirstBlock = true;
        public QuadSocketObject[] QuadSocketObjects { get; private set; }

        public event Action QuadBuild;
        public event Action QuadDestroy;
        public event Action OutsidePlayingArea;

        public void Initialize(
            QuadObject quadObject,
            QuadSocketObject quadSocketObject,
            QuadConfig[] quads,
            Canvas canvas,
            Transform releasedQuadsParent,
            RectTransform playZone,
            TowerHead towerHead,
            RectTransform canvasRectTransform,
            GarbageCollectorObject garbageCollectorObject)
        {
            this.quadObject = quadObject;
            this.quadSocketObject = quadSocketObject;
            this.quads = quads;
            this.canvas = canvas;
            this.releasedQuadsParent = releasedQuadsParent;
            this.playZone = playZone;
            this.canvasRectTransform = canvasRectTransform;
            this.garbageCollectorObject = garbageCollectorObject;
            this.towerHead = towerHead;

            quadsBuilder = new();
            quadsBuilder.Initialize(releasedQuadsParent, towerHead);

            quadsBuilder.TowerEmpty += TowerEmpty;
            quadsBuilder.MoveQuadTo += MoveQuadTo;
        }

        public void CreateQuadSockets()
        {
            QuadSocketObjects = new QuadSocketObject[quads.Length];

            for (int i = 0; i < quads.Length; i++)
            {
                var newQuadSocketObject = GameObject.Instantiate(quadSocketObject);
                newQuadSocketObject.Initialize(i, quads[i].Sprite, canvas, quadObject);
                newQuadSocketObject.CreateQuad();
                newQuadSocketObject.gameObject.SetActive(false);
                newQuadSocketObject.SocketRelease += OnQuadSocketReleased;

                QuadSocketObjects[i] = newQuadSocketObject;
            }
        }

        public void SetCurrentQuad(QuadObject quad)
        {
            currentQuadObject = quad;
            currentQuadObject.Release -= SetCurrentQuad;
            currentQuadObject.EndDrag -= OnQuadEndDragged;

            currentQuadObject.transform.SetParent(releasedQuadsParent);

            currentQuadObject.Release += SetCurrentQuad;
            currentQuadObject.EndDrag += OnQuadEndDragged;
        }

        private void OnQuadEndDragged()
        {
            if (currentQuadObject.IsTowerPart)
            {
                TowerPartHandler();
                return;
            }

            if(!UIObjectInsideCheck.IsFullyInside(playZone, currentQuadObject.RectTransform) || !UIObjectInsideCheck.IsFullyInside(canvasRectTransform, currentQuadObject.RectTransform))
            {
                currentQuadObject.Destroy += QuadDestroyed;

                currentQuadObject.DestroyAnimation();
                currentQuadObject = null;

                OutsidePlayingArea?.Invoke();
                return;
            }

            FreeQuadHandler();
        }

        private void TowerPartHandler()
        {
            if (UIObjectInsideCheck.IsInside(garbageCollectorObject.RectTransform, currentQuadObject.RectTransform))
            {
                currentQuadObject.Destroy += QuadDestroyed;

                currentQuadObject.MoveTo(garbageCollectorObject.RectTransform.position);
                currentQuadObject.DestroyAnimation();

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
                return;
            }

            if (UIObjectInsideCheck.IsInside(towerHead.rectTransform, currentQuadObject.RectTransform) && towerHead.rectTransform.position.y < currentQuadObject.RectTransform.position.y)
            {
                quadsBuilder.SetQuad(currentQuadObject);
                QuadBuild?.Invoke();
                return;
            }

            currentQuadObject.Destroy += QuadDestroyed;

            currentQuadObject.DestroyAnimation();
            currentQuadObject = null;
        }

        private void OnQuadSocketReleased(QuadObject quad)
        {
            SetCurrentQuad(quad);
        }

        private void QuadDestroyed(QuadObject quad)
        {
            quad.Destroy -= QuadDestroyed;

            QuadDestroy.Invoke();

            GameObject.Destroy(quad.gameObject);
        }

        public void Dispose()
        {
            quadsBuilder.TowerEmpty -= TowerEmpty;
            quadsBuilder.MoveQuadTo -= MoveQuadTo;
        }
    }
}