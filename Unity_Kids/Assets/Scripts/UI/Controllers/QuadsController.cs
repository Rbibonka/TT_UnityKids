using Configs;
using Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
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

        public event Action MessageQuadBuilded;
        public event Action MessageOutsidePlayingAreaWent;
        public event Action MessageQuadDestroyed;

        public event Action<QuadObject> QuadDestroyed;

        public QuadsController(
            Transform releasedQuadsParent,
            RectTransform playZone,
            TowerHead towerHead,
            RectTransform canvasRectTransform,
            GarbageCollectorObject garbageCollectorObject
            )
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

        public void AddSavedQuads(List<TowerQuad> towerQuads)
        {
            isFirstBlock = false;

            quadsBuilder.SetSavedQuads(towerQuads);
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

                currentQuadObject.DestroyWithAnimation();
                currentQuadObject = null;

                MessageOutsidePlayingAreaWent?.Invoke();
                return;
            }

            FreeQuadHandler();
        }

        private void TowerPartHandler()
        {
            if (UIObjectInsideCheck.IsEllipseTouchingRectangle(garbageCollectorObject.RectTransform, currentQuadObject.RectTransform))
            {
                currentQuadObject.Destroyed += OnQuadDestroyed;

                currentQuadObject.MoveTo(garbageCollectorObject.RectTransform.position);
                currentQuadObject.DestroyWithAnimation();

                MessageQuadDestroyed?.Invoke();

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
            quadObject.JumpWithAnimation(targetPoint);
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
                MessageQuadBuilded?.Invoke();
                return;
            }

            if (UIObjectInsideCheck.IsInside(towerHead.RectTransform, currentQuadObject.RectTransform)
                && towerHead.RectTransform.position.y < currentQuadObject.RectTransform.position.y)
            {
                if (quadsBuilder.Check�ompatibilityQuads(currentQuadObject))
                {
                    quadsBuilder.SetQuad(currentQuadObject);
                    MessageQuadBuilded?.Invoke();
                    return;
                }
            }

            currentQuadObject.Destroyed += OnQuadDestroyed;
            currentQuadObject.DestroyWithAnimation();
            currentQuadObject = null;

            MessageQuadDestroyed?.Invoke();
        }

        private void OnQuadDestroyed(QuadObject quad)
        {
            quad.Destroyed -= OnQuadDestroyed;

            QuadDestroyed?.Invoke(quad);
        }

        public List<TowerQuad> GetTowerQuads()
        {
            return quadsBuilder.GetTowerQuads();
        }

        public void Dispose()
        {
            quadsBuilder.TowerEmpty -= TowerEmpty;
            quadsBuilder.MoveQuadTo -= MoveQuadTo;
            quadsBuilder.JumpQuadTo -= JumpQuadTo;
        }
    }
}