using Configs;
using Objects;
using UnityEngine;

public class QuadsLifeCycleConrtoller
{
    private QuadConfig[] quads;
    private Canvas canvas;

    private QuadsObjectPool quadsObjectPool;

    public QuadsLifeCycleConrtoller(QuadConfig[] quads, QuadObject quadObject, Transform quadParent, Canvas canvas)
    {
        this.quads = quads;
        this.canvas = canvas;

        quadsObjectPool = new(quadObject, quadParent);
    }

    public QuadObject GetQuad(int quadId)
    {
        var quadObject = quadsObjectPool.GetFromPool();

        quadObject.Initialize(quadId, canvas);
        quadObject.SetSprite(quads[quadId].Sprite);

        return quadObject;
    }

    public void SetQuad(QuadObject quad)
    {
        quadsObjectPool.SetToPool(quad);
    }
}