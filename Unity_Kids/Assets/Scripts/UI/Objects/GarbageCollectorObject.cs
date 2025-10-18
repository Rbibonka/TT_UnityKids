using UnityEngine;

public class GarbageCollectorObject : MonoBehaviour
{
    [field: SerializeField]
    public RectTransform RectTransform { get; private set; }
}