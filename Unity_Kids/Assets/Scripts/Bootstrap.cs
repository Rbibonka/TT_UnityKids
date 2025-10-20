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

    [SerializeField]
    private LocalizationSetuper localizationSetuper;

    private void Awake()
    {
        JsonDataController jsonData = new();

        var savedQuads = jsonData.LoadData();

        var uiHandler = uiModel.GetHandler(gameConfig.Quads, gameConfig.QuadObject, gameConfig.QuadSocketObject, localizationSetuper, savedQuads);

        uiHandler.PrepareUI();
    }
}