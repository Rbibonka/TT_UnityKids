using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuadButtonModel : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private Image quadImage;

    private Sprite quadDefaultSprite;

    private int quadId;

    public event Action onClick;

    public void Initialize(Sprite quadDefaultSprite, int quadId)
    {
        this.quadDefaultSprite = quadDefaultSprite;
        this.quadId = quadId;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void SetDefaultSprite()
    {
        quadImage.sprite = quadDefaultSprite;
    }
}