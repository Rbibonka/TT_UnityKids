using Configs;
using Objects;
using System;
using UnityEngine;

namespace Handlers
{
    public class QuadSocketsHandler
    {
        private Canvas canvas;
        private QuadObject quadObject;
        private QuadConfig[] quads;
        private QuadSocketObject quadSocketObject;

        public event Action<QuadObject> QuadReleased;

        public QuadSocketsHandler(QuadConfig[] quads, QuadSocketObject quadSocketObject, QuadObject quadObject, Canvas canvas)
        {
            this.quads = quads;
            this.quadSocketObject = quadSocketObject;
            this.quadObject = quadObject;
            this.canvas = canvas;
        }

        public QuadSocketObject[] CreateQuadSockets()
        {
            var quadSocketObjects = new QuadSocketObject[quads.Length];

            for (int i = 0; i < quads.Length; i++)
            {
                var newQuadSocketObject = GameObject.Instantiate(quadSocketObject);
                newQuadSocketObject.Initialize(i, quads[i].Sprite, canvas, quadObject);
                newQuadSocketObject.CreateQuad();
                newQuadSocketObject.gameObject.SetActive(false);
                newQuadSocketObject.SocketRelease += OnQuadSocketReleased;

                quadSocketObjects[i] = newQuadSocketObject;
            }

            return quadSocketObjects;
        }

        private void OnQuadSocketReleased(QuadObject quadObject)
        {
            QuadReleased?.Invoke(quadObject);
        }
    }
}