using DG.Tweening;
using System;
using UnityEngine;

namespace Objects
{
    public class QuadSocketObject : MonoBehaviour
    {
        private int quadSocketId;
        private Sprite quadSprite;
        private Canvas canvas;
        private QuadObject quadObject;

        private QuadObject currentQuadObject;

        public event Action<QuadObject> SocketRelease;

        public void Initialize(int quadSocketId, Sprite quadSprite, Canvas canvas, QuadObject quadObject)
        {
            this.quadSocketId = quadSocketId;
            this.quadSprite = quadSprite;
            this.canvas = canvas;
            this.quadObject = quadObject;
        }

        public void CreateQuad()
        {
            currentQuadObject = Instantiate(quadObject, transform);
            currentQuadObject.Initialize(quadSocketId, canvas);
            currentQuadObject.SetSprite(quadSprite);

            currentQuadObject.Release += OnReleased;
        }

        private void OnReleased(QuadObject quadObject)
        {
            currentQuadObject.Release -= OnReleased;
            currentQuadObject = null;

            CreateQuad();
            currentQuadObject.SpawnAnimation();

            SocketRelease.Invoke(quadObject);
        }
    }
}