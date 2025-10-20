using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Configs;

public class JsonDataController
{
    private string filePath;

    public JsonDataController()
    {
        filePath = Path.Combine(Application.persistentDataPath, "data.json");
    }

    public void SaveData(List<TowerQuad> dataList)
    {
        string json = JsonUtility.ToJson(new Wrapper<TowerQuad> { Items = dataList }, true);
        File.WriteAllText(filePath, json);
    }

    public List<TowerQuad> LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Wrapper<TowerQuad> wrapper = JsonUtility.FromJson<Wrapper<TowerQuad>>(json);
            return wrapper.Items;
        }
        else
        {
            Debug.LogWarning("File does not exist");
            return new List<TowerQuad>();
        }
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}