using System;
using UnityEngine;

namespace Objects
{
    public sealed class QuadSocketObject : MonoBehaviour
    {
        private int quadSocketId;
        private Sprite quadSprite;
        private Canvas canvas;
        private QuadObject quadObject;
        private Transform socketParent;

        private QuadObject currentQuadObject;

        public event Action<QuadObject> SocketRelease;

        public event Action<int> SocketEmpty;

        public void Initialize(int quadSocketId, Sprite quadSprite, Canvas canvas, QuadObject quadObject, Transform socketParent)
        {
            this.quadSocketId = quadSocketId;
            this.quadSprite = quadSprite;
            this.canvas = canvas;
            this.quadObject = quadObject;
            this.socketParent = socketParent;
        }

        public void SetQuad(QuadObject quadObject)
        {
            currentQuadObject = quadObject;
            currentQuadObject.RectTransform.position = socketParent.position;
            currentQuadObject.transform.SetParent(socketParent);

            currentQuadObject.Initialize(quadSocketId, canvas);
            currentQuadObject.SetSprite(quadSprite);

            currentQuadObject.SpawnAnimation();
            currentQuadObject.BeginDragged += OnBeginDragged;
        }

        private void OnBeginDragged(QuadObject quadObject)
        {
            currentQuadObject.BeginDragged -= OnBeginDragged;
            currentQuadObject.DeleteFromSocket();
            currentQuadObject = null;

            SocketEmpty.Invoke(quadSocketId);
            SocketRelease.Invoke(quadObject);
        }
    }
}