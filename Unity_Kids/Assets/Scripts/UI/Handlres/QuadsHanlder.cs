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

        private QuadsBuilder quadsBuilder;

        private QuadObject currentQuadObject;

        private bool isFirstBlock = true;
        UIObjectInsideCheck uiObjectInsideCheck;

        public QuadSocketObject[] QuadSocketObjects { get; private set; }

        public event Action<QuadObject> QuadSocketReleased;
        public event Action<QuadObject> QuadBuilded;

        public void Initialize(
            QuadObject quadObject,
            QuadSocketObject quadSocketObject,
            QuadConfig[] quads,
            Canvas canvas,
            Transform releasedQuadsParent,
            RectTransform playZone,
            TowerHead towerHead,
            RectTransform canvasRectTransform)
        {
            this.quadObject = quadObject;
            this.quadSocketObject = quadSocketObject;
            this.quads = quads;
            this.canvas = canvas;
            this.releasedQuadsParent = releasedQuadsParent;
            this.playZone = playZone;
            this.canvasRectTransform = canvasRectTransform;

            quadsBuilder = new();
            quadsBuilder.Initialize(releasedQuadsParent, towerHead);

            quadsBuilder.TowerEmpty += TowerEmpty;
            quadsBuilder.MoveQuadTo += MoveQuadTo;

            uiObjectInsideCheck = new();
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

            if(!uiObjectInsideCheck.IsFullyInside(playZone, currentQuadObject.RectTransform) || !uiObjectInsideCheck.IsFullyInside(canvasRectTransform, currentQuadObject.RectTransform))
            {
                currentQuadObject.DestroyAnimation();
                currentQuadObject = null;
                return;
            }

            FreeQuadHandler();
        }

        private void TowerPartHandler()
        {
            var collisions = currentQuadObject.TakeCollision();

            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent<GarbageObject>(out GarbageObject garbageObject))
                {
                    currentQuadObject.Destroy += TowerPartDestroyed;

                    currentQuadObject.MoveTo(garbageObject.RectTransform.position);
                    currentQuadObject.DestroyAnimation();

                    quadsBuilder.DeleteQuad(currentQuadObject);
                    return;
                }
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

            var collisions = currentQuadObject.TakeCollision();

            foreach (var collision in collisions)
            {
                if (collision.TryGetComponent<TowerHead>(out TowerHead towerHead))
                {
                    quadsBuilder.SetQuad(currentQuadObject);
                    return;
                }
            }

            currentQuadObject.DestroyAnimation();
            currentQuadObject = null;
        }

        private void OnQuadSocketReleased(QuadObject quad)
        {
            QuadSocketReleased?.Invoke(quad);
        }

        private void TowerPartDestroyed(QuadObject quad)
        {
            quad.Destroy -= TowerPartDestroyed;

            GameObject.Destroy(quad);
        }

        public void Dispose()
        {
            quadsBuilder.TowerEmpty -= TowerEmpty;
            quadsBuilder.MoveQuadTo -= MoveQuadTo;
        }
    }
}