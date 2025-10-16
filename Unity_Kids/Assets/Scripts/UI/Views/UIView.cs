using UnityEngine;

public sealed class UIView : MonoBehaviour
{
    [SerializeField]
    private ScrollBarView scrollBarView;

    private UIHandler uiHandler;

    private ScrollBarHandler scrollBarHandler;

    public UIHandler GetHandler(QuadConfig[] quads)
    {
        if (uiHandler == null || !uiHandler.IsInitilized)
        {
            scrollBarHandler = scrollBarView.GetHandler(quads);

            uiHandler = new();
            uiHandler.Initialize(scrollBarHandler);
        }

        return uiHandler;
    }
}