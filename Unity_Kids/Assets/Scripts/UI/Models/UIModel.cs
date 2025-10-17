using UnityEngine;

public sealed class UIModel : MonoBehaviour
{
    [SerializeField]
    private ScrollBarModel scrollBarModel;

    [SerializeField]
    private QuadButtonModel quadButtonModel;

    private UIHandler uiHandler;

    private ScrollBarHandler scrollBarHandler;

    public UIHandler GetHandler(QuadConfig[] quads)
    {
        if (uiHandler == null || !uiHandler.IsInitilized)
        {
            scrollBarHandler = scrollBarModel.GetHandler();

            uiHandler = new();
            uiHandler.Initialize(quadButtonModel, scrollBarHandler, quads);
        }

        return uiHandler;
    }
}