using UnityEngine;

[DefaultExecutionOrder(-1)]
public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private UIModel uiModel;

    [SerializeField]
    private MainGameConfig gameConfig;

    private void Awake()
    {
        var uiHandler = uiModel.GetHandler(gameConfig.Quads);

        uiHandler.Loop();
    }
}