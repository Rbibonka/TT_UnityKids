using UnityEngine;

public sealed class UIHandler
{
    private ScrollBarHandler scrollBarHandler;

    public bool IsInitilized { get; private set; }

    public void Initialize(ScrollBarHandler scrollBarHandler)
    {
        this.scrollBarHandler = scrollBarHandler;
    }

    public void FillContent()
    {
        scrollBarHandler.FillQuadButtons();
    }
}