using UnityEngine;
using UnityEngine.UI;

public sealed class ScrollBarView : MonoBehaviour
{
    [SerializeField]
    private RectTransform contentParent;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private QuadButtonView buttonQuad;

    private ScrollBarHandler scrollBarHandler;

    public ScrollBarHandler GetHandler(QuadConfig[] quads)
    {
        if (scrollBarHandler == null || !scrollBarHandler.IsInitilized)
        {
            scrollBarHandler = new();
            scrollBarHandler.Initialize(contentParent, quads, buttonQuad);
        }

        return scrollBarHandler;
    }
}