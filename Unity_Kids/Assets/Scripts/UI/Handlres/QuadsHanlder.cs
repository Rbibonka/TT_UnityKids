using UnityEngine;
using Objects;
using Configs;
using System;

namespace Handlers
{
    public sealed class QuadsHanlder : IDisposable
    {
        private QuadConfig[] quads;
        private Canvas canvas;
        private QuadObject quadObject;
        private QuadSocketObject quadSocketObject;
        private Transform releasedQuadsParent;

        public QuadSocketObject[] QuadSocketObjects { get; private set; }

        public event Action<QuadObject> QuadSocketReleased;

        public void Initialize(
            QuadObject quadObject,
            QuadSocketObject quadSocketObject,
            QuadConfig[] quads,
            Canvas canvas,
            Transform releasedQuadsParent)
        {
            this.quadObject = quadObject;
            this.quadSocketObject = quadSocketObject;
            this.quads = quads;
            this.canvas = canvas;
            this.releasedQuadsParent = releasedQuadsParent;
        }

        public void CreateQuadSockets()
        {
            QuadSocketObjects = new QuadSocketObject[quads.Length];

            for (int i = 0; i < quads.Length; i++)
            {
                var newQuadSocketObject = GameObject.Instantiate(quadSocketObject);
                newQuadSocketObject.Initialize(i, quads[i].Sprite, canvas, quadObject);
                newQuadSocketObject.CreateQuad();
                newQuadSocketObject.gameObject.SetActive(false);
                newQuadSocketObject.SocketRelease += OnQuadSocketReleased;

                QuadSocketObjects[i] = newQuadSocketObject;
            }
        }

        public void DeactivateButtonQuads()
        {
            
        }

        public void ActivateButtonQuads()
        {
            
        }

        public void MoveQuadBlock(QuadObject quad)
        {
            quad.transform.SetParent(releasedQuadsParent);
        }

        private void OnQuadSocketReleased(QuadObject quad)
        {
            QuadSocketReleased?.Invoke(quad);
        }

        public void Dispose()
        {
        }
    }
}