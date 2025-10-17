using UnityEngine;

public class QuadHanlder
{
    private QuadConfig[] quads;
    private QuadButtonModel quadButtonModel;

    public QuadButtonModel[] quadButtons { get; private set; }

    public void Initialize(QuadButtonModel quadButtonModel, QuadConfig[] quads)
    {
        this.quadButtonModel = quadButtonModel;
        this.quads = quads;
    }

    public void CreateQuadButtons()
    {
        quadButtons = new QuadButtonModel[quads.Length];

        for (int i = 0; i < quads.Length; i++)
        {
            var newButtonQuad = GameObject.Instantiate(quadButtonModel);
            newButtonQuad.Initialize(quads[i].Sprite, i);
            newButtonQuad.SetDefaultSprite();

            newButtonQuad.gameObject.SetActive(false);

            quadButtons[i] = newButtonQuad;
        }
    }
}