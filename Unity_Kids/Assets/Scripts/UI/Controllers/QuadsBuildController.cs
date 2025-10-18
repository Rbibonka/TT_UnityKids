using Configs;
using Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    public struct TowerQuad
    {
        public Vector3 position;
        public QuadObject quadObject;
    }
}

namespace Controllers
{
    public sealed class QuadsBuildController
    {
        private Transform quadsTransformParent;

        private List<TowerQuad> quads = new List<TowerQuad>();

        private TowerHead towerHead;

        public event Action TowerEmpty;
        public event Action<QuadObject, Vector3> MoveQuadTo;
        public event Action<QuadObject, Vector3> JumpQuadTo;

        public QuadsBuildController(Transform quadsTransformParent, TowerHead towerHead)
        {
            this.quadsTransformParent = quadsTransformParent;
            this.towerHead = towerHead;
        }

        public Vector3 GetQuadTowerPosition(QuadObject quadObject)
        {
            return quads.FindLast(quad => quad.quadObject == quadObject).position;
        }

        public bool CheckÑompatibilityQuads(QuadObject quadObject)
        {
            // TODO: The place where you can check the possibility of installing a quad.
            return true;
        }

        public void SetFirstQuad(QuadObject quadObject)
        {
            TowerQuad towerQuad = new TowerQuad()
            {
                quadObject = quadObject,
                position = quadObject.RectTransform.position,
            };

            var quad = AddQuad(towerQuad);
            UpdateTowerHeadPosition(quad.quadObject.RectTransform);
        }

        public void SetQuad(QuadObject quadObject)
        {
            TowerQuad towerQuad = new TowerQuad()
            {
                quadObject = quadObject,
                position = GetHighterTowerPoint(),
            };

            var quad = AddQuad(towerQuad);

            quad.quadObject.JumpAnimationEnd += OnJumpAnimationEnd;
            JumpQuadTo.Invoke(quad.quadObject, quad.position);
        }

        public void DeleteQuad(QuadObject quadObject)
        {
            var quadDelete = quads.FindIndex(index => index.quadObject == quadObject);

            for (int i = quadDelete + 1; i < quads.Count; i++)
            {
                var quad = quads[i - 1];
                quad.quadObject = quads[i].quadObject;

                quads[i - 1] = quad;
            }

            quads.RemoveAt(quads.Count - 1);

            for (int i = quadDelete; i < quads.Count; i++)
            {
                MoveQuadTo?.Invoke(quads[i].quadObject, quads[i].position);
            }

            if (quads.Count == 0)
            {
                towerHead.transform.SetParent(quadsTransformParent);
                TowerEmpty?.Invoke();

                return;
            }

            UpdateTowerHeadPosition(quads[quads.Count - 1].quadObject.RectTransform);
        }

        private TowerQuad AddQuad(TowerQuad towerQuad)
        {
            quads.Add(towerQuad);
            towerQuad.quadObject.SetAsTowerPart();

            return quads[quads.Count - 1];
        }

        private void OnJumpAnimationEnd()
        {
            var quad = quads[quads.Count - 1];

            quad.quadObject.JumpAnimationEnd -= OnJumpAnimationEnd;
            UpdateTowerHeadPosition(quad.quadObject.RectTransform);
        }

        private Vector3 GetHighterTowerPoint()
        {
            Vector2 towerHeadSize = towerHead.RectTransform.rect.size;
            float scaleFactorY = towerHead.RectTransform.lossyScale.y;
            float towerHeadHeight = towerHeadSize.y * scaleFactorY;
            float offset = towerHeadHeight * (1 - towerHead.RectTransform.pivot.y);

            float scaleX = towerHead.RectTransform.lossyScale.x;
            float width = towerHeadSize.x * scaleX;

            float maxOffsetX = 0.5f * width;
            float randomOffsetX = UnityEngine.Random.Range(-maxOffsetX, maxOffsetX);

            return towerHead.RectTransform.position + new Vector3(randomOffsetX, offset, 0);
        }

        private void UpdateTowerHeadPosition(RectTransform lastQuadRectTransform)
        {
            Vector2 size1 = lastQuadRectTransform.rect.size;
            Vector2 size2 = towerHead.RectTransform.rect.size;

            float scaleFactorX = lastQuadRectTransform.lossyScale.x;
            float scaleFactorY = towerHead.RectTransform.lossyScale.y;

            float height1 = size1.y * scaleFactorY;
            float height2 = size2.y * scaleFactorY;

            float offsetY1 = height1 * lastQuadRectTransform.pivot.y;
            float offsetY2 = height2 * (1 - towerHead.RectTransform.pivot.y);

            Vector3 topPosition = lastQuadRectTransform.position + new Vector3(0, offsetY1 + offsetY2, 0);
            towerHead.RectTransform.position = topPosition;

            towerHead.transform.SetParent(lastQuadRectTransform);
        }
    }
}