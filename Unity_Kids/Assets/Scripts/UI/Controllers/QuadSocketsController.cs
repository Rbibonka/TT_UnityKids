using Configs;
using JetBrains.Annotations;
using Objects;
using System;
using UnityEngine;

namespace Controller
{
    public sealed class QuadSocketsController : IDisposable
    {
        private Canvas canvas;
        private QuadObject quadObject;
        private QuadConfig[] quads;
        private QuadSocketObject quadSocketObject;
        private QuadSocketObject[] quadSocketObjects;

        public event Action<QuadObject> QuadReleased;
        public event Action<int> SocketEmpty;

        public QuadSocketsController(QuadConfig[] quads, QuadSocketObject quadSocketObject, QuadObject quadObject, Canvas canvas)
        {
            this.quads = quads;
            this.quadSocketObject = quadSocketObject;
            this.quadObject = quadObject;
            this.canvas = canvas;
        }

        public QuadSocketObject[] CreateQuadSockets()
        {
            quadSocketObjects = new QuadSocketObject[quads.Length];

            for (int i = 0; i < quads.Length; i++)
            {
                var newQuadSocketObject = GameObject.Instantiate(quadSocketObject);
                newQuadSocketObject.Initialize(i, quads[i].Sprite, canvas, quadObject, newQuadSocketObject.transform);
                newQuadSocketObject.gameObject.SetActive(false);
                newQuadSocketObject.SocketRelease += OnQuadSocketReleased;
                newQuadSocketObject.SocketEmpty += OnSocketEmpty;

                quadSocketObjects[i] = newQuadSocketObject;
            }

            return quadSocketObjects;
        }

        public void SetQuadToSocket(int socketId, QuadObject quad)
        {
            quadSocketObjects[socketId].SetQuad(quad);
        }

        private void OnSocketEmpty(int socketId)
        {
            SocketEmpty?.Invoke(socketId);
        }

        private void OnQuadSocketReleased(QuadObject quadObject)
        {
            QuadReleased?.Invoke(quadObject);
        }

        public void Dispose()
        {
            foreach (var quadSocketObject in quadSocketObjects)
            {
                quadSocketObject.SocketRelease -= OnQuadSocketReleased;
                quadSocketObject.SocketEmpty -= OnSocketEmpty;
            }
        }
    }
}