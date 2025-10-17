using UnityEngine;
using UnityEngine.UI;

public sealed class ScrollBarModel : MonoBehaviour
{
    [SerializeField]
    private RectTransform contentParent;

    [SerializeField]
    private ScrollRect scrollRect;

    private ScrollBarHandler scrollBarHandler;

    public ScrollBarHandler GetHandler()
    {
        if (scrollBarHandler == null || !scrollBarHandler.IsInitilized)
        {
            scrollBarHandler = new();
            scrollBarHandler.Initialize(contentParent);
        }

        return scrollBarHandler;
    }
}