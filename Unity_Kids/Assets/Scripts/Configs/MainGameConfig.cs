using UnityEngine;

[CreateAssetMenu(fileName = "MainConfig", menuName = "Game/MainConfig", order = 51)]
public sealed class MainGameConfig : ScriptableObject
{
    [field: SerializeField]
    public QuadConfig[] Quads { get; private set; }
}