using UnityEngine;
using Models;
using Configs;

[DefaultExecutionOrder(-1)]
public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private UIModel uiModel;

    [SerializeField]
    private MainGameConfig gameConfig;

    private void Awake()
    {
        var uiHandler = uiModel.GetHandler(gameConfig.Quads, gameConfig.QuadObject, gameConfig.QuadSocketObject);

        uiHandler.Loop();
    }
}