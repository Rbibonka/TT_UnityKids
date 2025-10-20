using Objects;
using UnityEngine;

public class QuadsObjectPool : BaseObjectPool<QuadObject>
{
    public QuadsObjectPool(QuadObject quadPrefab, Transform parent) : base(quadPrefab, parent) { }
}