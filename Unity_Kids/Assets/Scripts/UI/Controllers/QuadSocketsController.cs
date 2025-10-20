using Configs;
using JetBrains.Annotations;
using Objects;
using System;
using UnityEngine;

namespace Controller
{
    public sealed class QuadSocketsController : IDisposable
    {
        private QuadConfig[] quads;
        private QuadSocketObject quadSocketObject;
        private QuadSocketObject[] quadSocketObjects;
        private QuadsLifeCycleConrtoller quadsCreator;

        public event Action<QuadObject> QuadReleased;
        public event Action<int> SocketEmpty;

        public QuadSocketsController(QuadConfig[] quads, QuadSocketObject quadSocketObject, QuadsLifeCycleConrtoller quadsCreator)
        {
            this.quads = quads;
            this.quadSocketObject = quadSocketObject;
            this.quadsCreator = quadsCreator;
        }

        public QuadSocketObject[] CreateQuadSockets()
        {
            quadSocketObjects = new QuadSocketObject[quads.Length];

            for (int i = 0; i < quads.Length; i++)
            {
                var newQuadSocketObject = GameObject.Instantiate(quadSocketObject);
                newQuadSocketObject.Initialize(i, newQuadSocketObject.transform);
                newQuadSocketObject.gameObject.SetActive(false);
                newQuadSocketObject.SocketRelease += OnQuadSocketReleased;
                newQuadSocketObject.SocketEmpty += OnSocketEmpty;

                quadSocketObjects[i] = newQuadSocketObject;
            }

            for (int i = 0; i < quads.Length; i++)
            {
                SetQuadToSocket(i);
            }

            return quadSocketObjects;
        }

        public void SetQuadToSocket(int socketId)
        {
            quadSocketObjects[socketId].SetQuad(quadsCreator.GetQuad(socketId));
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