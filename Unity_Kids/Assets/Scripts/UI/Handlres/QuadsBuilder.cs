using Cysharp.Threading.Tasks.Triggers;
using Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct TowerQuad
{
    public Vector3 position;
    public QuadObject quadObject;
}

public sealed class QuadsBuilder
{
    private Transform quadsTransformParent;

    private List<TowerQuad> quads = new List<TowerQuad>();

    private TowerHead towerHead;

    public event Action TowerEmpty;

    public event Action<QuadObject, Vector3> MoveQuadTo;

    public Vector3 GetQuadTowerPosition(QuadObject quadObject)
    {
        return quads.FindLast(quad => quad.quadObject == quadObject).position;
    }

    public void Initialize(Transform quadsTransformParent, TowerHead towerHead)
    {
        this.quadsTransformParent = quadsTransformParent;
        this.towerHead = towerHead;
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

        quad.quadObject.JumpAnimationEnd += OnAnimationEnd;
        quad.quadObject.JumpAnimation(quad.position);
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

    private void OnAnimationEnd()
    {
        var quad = quads[quads.Count - 1];

        quad.quadObject.JumpAnimationEnd -= OnAnimationEnd;
        UpdateTowerHeadPosition(quad.quadObject.RectTransform);
    }

    private Vector3 GetHighterTowerPoint()
    {
        Vector2 size2 = towerHead.rectTransform.rect.size;
        float scaleFactorY = towerHead.rectTransform.lossyScale.y;
        float height2 = size2.y * scaleFactorY;
        float offsetY2 = height2 * (1 - towerHead.rectTransform.pivot.y);

        float scaleX = towerHead.rectTransform.lossyScale.x;
        float width = size2.x * scaleX;

        float maxOffsetX = 0.5f * width;
        float randomOffsetX = UnityEngine.Random.Range(-maxOffsetX, maxOffsetX);

        return towerHead.rectTransform.position + new Vector3(randomOffsetX, offsetY2, 0);
    }

    private void UpdateTowerHeadPosition(RectTransform lastQuadRectTransform)
    {
        Vector2 size1 = lastQuadRectTransform.rect.size;
        Vector2 size2 = towerHead.rectTransform.rect.size;

        float scaleFactorX = lastQuadRectTransform.lossyScale.x;
        float scaleFactorY = towerHead.rectTransform.lossyScale.y;

        float height1 = size1.y * scaleFactorY;
        float height2 = size2.y * scaleFactorY;

        float offsetY1 = height1 * lastQuadRectTransform.pivot.y;
        float offsetY2 = height2 * (1 - towerHead.rectTransform.pivot.y);

        Vector3 topPosition = lastQuadRectTransform.position + new Vector3(0, offsetY1 + offsetY2, 0);
        towerHead.rectTransform.position = topPosition;

        towerHead.transform.SetParent(lastQuadRectTransform);
    }
}