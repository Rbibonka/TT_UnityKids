using System;
using UnityEngine;

public enum Localization
{
    Ru,
    Eng
}

[Serializable]
public struct LocalizationConfig
{
    [SerializeField]
    public string QuadBuildMessage;

    [SerializeField]
    public string QuadDestroyMessage;

    [SerializeField]
    public string OutsidePlayingAreMessage;
}

public class LocalizationSetuper : MonoBehaviour
{
    [SerializeField]
    public Localization currentLocalization;

    [SerializeField]
    private LocalizationConfig configRu;

    [SerializeField]
    private LocalizationConfig configEng;

    private LocalizationConfig currrentLocalizationConfig;

    public LocalizationConfig GetCurrentLocalizationConfig()
    {
        return currrentLocalizationConfig;
    }

    public void SetCurrentLocalization(Localization localization)
    {
        if (localization == Localization.Eng)
        {
            currrentLocalizationConfig = configEng;
        }
        else if (localization == Localization.Ru)
        {
            currrentLocalizationConfig = configRu;
        }
        else
        {
            Debug.Log("There is no localization");
        }
    }
}