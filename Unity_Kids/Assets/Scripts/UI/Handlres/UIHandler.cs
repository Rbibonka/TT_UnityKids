using UnityEngine;

public sealed class UIHandler
{
    private ScrollBarHandler scrollBarHandler;

    private QuadHanlder quadHanlder;

    private QuadConfig[] quads;

    public bool IsInitilized { get; private set; }

    public void Initialize(QuadButtonModel quadButtonModel, ScrollBarHandler scrollBarHandler, QuadConfig[] quads)
    {
        this.scrollBarHandler = scrollBarHandler;
        this.quads = quads;

        quadHanlder = new();
        quadHanlder.Initialize(quadButtonModel, quads);
    }

    public void Loop()
    {
        CreateButtonsQuad();
    }

    private void CreateButtonsQuad()
    {
        quadHanlder.CreateQuadButtons();
        scrollBarHandler.FillQuadButtons(quadHanlder.quadButtons);
    }
}