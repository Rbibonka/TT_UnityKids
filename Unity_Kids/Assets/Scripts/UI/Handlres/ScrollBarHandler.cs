using UnityEngine;

public sealed class ScrollBarHandler
{
    private RectTransform contentParent;

    private QuadConfig[] quads;
    private QuadButtonView buttonQuadPrefab;

    public bool IsInitilized { get; private set; }

    public void Initialize(RectTransform contentParent, QuadConfig[] quads, QuadButtonView buttonQuadPrefab)
    {
        this.contentParent = contentParent;
        this.buttonQuadPrefab = buttonQuadPrefab;
        this.quads = quads;

        IsInitilized = true;
    }

    public void FillQuadButtons()
    {
        foreach (var quad in quads)
        {
            var newButtonQuad = GameObject.Instantiate(buttonQuadPrefab, contentParent);
            newButtonQuad.Initialize(quad.Sprite, quad.Id);
        }
    }
}