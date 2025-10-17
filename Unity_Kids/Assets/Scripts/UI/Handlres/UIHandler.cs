using Configs;
using Objects;
using UnityEngine;

namespace Handlers
{
    public sealed class UIHandler
    {
        private ScrollBarHandler scrollBarHandler;

        private QuadsHanlder quadsHandler;

        public bool IsInitilized { get; private set; }

        private QuadsBuilder quadsBuilder;

        public void Initialize(
            QuadObject quadButtonObject,
            QuadSocketObject quadSocketObject,
            ScrollBarHandler scrollBarHandler,
            QuadConfig[] quads,
            Canvas canvas,
            Transform releasedQuadsParent)
        {
            this.scrollBarHandler = scrollBarHandler;

            quadsHandler = new();
            quadsHandler.Initialize(quadButtonObject, quadSocketObject, quads, canvas, releasedQuadsParent);

            quadsHandler.QuadSocketReleased += OnQuadSocketReleased;
        }

        public void Loop()
        {
            CreateButtonsQuad();
        }

        private void CreateButtonsQuad()
        {
            quadsHandler.CreateQuadSockets();
            scrollBarHandler.FillQuadButtons(quadsHandler.QuadSocketObjects);
        }

        private void OnQuadSocketReleased(QuadObject quadButtonId)
        {
            quadsHandler.MoveQuadBlock(quadButtonId);
        }
    }
}