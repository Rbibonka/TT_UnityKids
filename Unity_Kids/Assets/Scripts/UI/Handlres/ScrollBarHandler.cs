using UnityEngine;

public sealed class ScrollBarHandler
{
    private RectTransform contentParent;

    public bool IsInitilized { get; private set; }

    public void Initialize(RectTransform contentParent)
    {
        this.contentParent = contentParent;

        IsInitilized = true;
    }

    public void FillQuadButtons(QuadButtonModel[] quadButtons)
    {
        foreach (var quadButton in quadButtons)
        {
            quadButton.transform.SetParent(contentParent, false);
            quadButton.gameObject.SetActive(true);
        }
    }
}