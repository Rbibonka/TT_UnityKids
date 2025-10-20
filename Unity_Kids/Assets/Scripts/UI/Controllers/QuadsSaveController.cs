using Configs;
using Objects;
using System;
using System.Collections.Generic;

public class QuadsSaveController : IDisposable
{
    private QuadsLifeCycleConrtoller quadsLifeCycleConrtoller;
    private List<TowerQuad> savedQuads;

    public event Action<QuadObject> BeginDragged;

    public QuadsSaveController(QuadsLifeCycleConrtoller quadsLifeCycleConrtoller, List<TowerQuad> savedQuads)
    {
        this.quadsLifeCycleConrtoller = quadsLifeCycleConrtoller;
        this.savedQuads = savedQuads;
    }

    public List<TowerQuad> CreateSavedQuads()
    {
        for (int i = 0; i < savedQuads.Count; i++)
        {
            var quad = quadsLifeCycleConrtoller.GetQuad(savedQuads[i].quadId);

            var saveQuad = new TowerQuad()
            {
                position = savedQuads[i].position,
                quadId = savedQuads[i].quadId,
                quadObject = quad,
            };

            savedQuads[i] = saveQuad;

            savedQuads[i].quadObject.BeginDragged += OnBeginDragged;
        }

        return savedQuads;
    }

    private void OnBeginDragged(QuadObject quad)
    {
        BeginDragged?.Invoke(quad);
    }

    public void Dispose()
    {
        for (int i = 0; i < savedQuads.Count - 1; i++)
        {
            savedQuads[i].quadObject.BeginDragged += OnBeginDragged;
        }
    }
}