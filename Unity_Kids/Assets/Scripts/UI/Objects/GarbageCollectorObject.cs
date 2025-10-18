using UnityEngine;

public sealed class GarbageCollectorObject : MonoBehaviour
{
    [field: SerializeField]
    public RectTransform RectTransform { get; private set; }
}