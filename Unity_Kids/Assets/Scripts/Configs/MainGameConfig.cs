using Objects;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "Game/MainConfig", order = 51)]
    public sealed class MainGameConfig : ScriptableObject
    {
        [field: SerializeField]
        public QuadObject QuadObject { get; private set; }

        [field: SerializeField]
        public QuadSocketObject QuadSocketObject { get; private set; }

        [field: SerializeField]
        public QuadConfig[] Quads { get; private set; }

    }
}