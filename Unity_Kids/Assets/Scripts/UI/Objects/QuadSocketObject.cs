using System;
using UnityEngine;

namespace Objects
{
    public sealed class QuadSocketObject : MonoBehaviour
    {
        private int quadSocketId;
        private Transform socketParent;

        private QuadObject currentQuadObject;

        public event Action<QuadObject> SocketRelease;

        public event Action<int> SocketEmpty;

        public void Initialize(int quadSocketId, Transform socketParent)
        {
            this.quadSocketId = quadSocketId;
            this.socketParent = socketParent;
        }

        public void SetQuad(QuadObject quadObject)
        {
            currentQuadObject = quadObject;
            currentQuadObject.RectTransform.position = socketParent.position;
            currentQuadObject.transform.SetParent(socketParent);

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