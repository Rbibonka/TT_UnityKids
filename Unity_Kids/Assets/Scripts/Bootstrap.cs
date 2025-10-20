using UnityEngine;
using Models;
using Configs;
using Controllers;

[DefaultExecutionOrder(-1)]
public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField]
    private UIModel uiModel;

    [SerializeField]
    private MainGameConfig gameConfig;

    [SerializeField]
    private LocalizationSetuper localizationSetuper;

    private JsonDataController jsonData;
    private UIController controller;

    private void Awake()
    {
        jsonData = new();
        var savedQuads = jsonData.LoadData();

        controller = uiModel.GetHandler(gameConfig.Quads, gameConfig.QuadObject, gameConfig.QuadSocketObject, localizationSetuper, savedQuads);

        controller.PrepareUI();
    }

    private void OnDisable()
    {
        jsonData.SaveData(controller.GetTowerQuads());
        controller.Dispose();
    }
}