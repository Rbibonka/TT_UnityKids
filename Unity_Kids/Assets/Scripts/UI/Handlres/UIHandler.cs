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

        public void Initialize(QuadObject quadButtonObject, QuadSocketObject quadSocketObject, ScrollBarHandler scrollBarHandler, QuadConfig[] quads, Canvas canvas)
        {
            this.scrollBarHandler = scrollBarHandler;

            quadsHandler = new();
            quadsHandler.Initialize(quadButtonObject, quadSocketObject, quads, canvas);

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

        private void QuadButtonClick(int quadButtonId)
        {
           // quadsHandler.CreateQuadBlock(quadButtonId);
            //quadsHandler.DeactivateButtonQuads();

            //scrollBarHandler.Deactivate();
        }

        private void OnQuadSocketReleased(int quadButtonId)
        {
            quadsHandler.CreateQuadBlock(quadButtonId);
        }
    }
}