using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuadButtonView : MonoBehaviour
{
    private Sprite quadSprite;

    private int quadId;

    public event Action onClick;

    public void Initialize(Sprite quadSprite, int quadId)
    {
        this.quadSprite = quadSprite;
        this.quadId = quadId;
    }
}