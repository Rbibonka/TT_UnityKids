using Configs;
using Objects;
using TMPro;
using UnityEngine;

namespace Handlers
{
    public sealed class UIHandler
    {
        private ScrollBarHandler scrollBarHandler;

        private QuadsHanlder quadsHandler;
        private LocalizationSetuper localizationSetuper;

        private MessageShower messageShower;

        public bool IsInitilized { get; private set; }

        public void Initialize(
            QuadObject quadButtonObject,
            QuadSocketObject quadSocketObject,
            ScrollBarHandler scrollBarHandler,
            QuadConfig[] quads,
            Canvas canvas,
            Transform releasedQuadsParent,
            RectTransform playZone,
            TowerHead towerHead,
            RectTransform canvasRectTransform,
            TMP_Text txt_Message,
            LocalizationSetuper localizationSetuper,
            GarbageCollectorObject garbageCollectorObject)
        {
            this.scrollBarHandler = scrollBarHandler;
            this.localizationSetuper = localizationSetuper;

            quadsHandler = new();
            quadsHandler.Initialize(quadButtonObject, quadSocketObject, quads, canvas, releasedQuadsParent, playZone, towerHead, canvasRectTransform, garbageCollectorObject);

            messageShower = new(txt_Message);

            quadsHandler.QuadBuild += OnQuadBuilded;
            quadsHandler.QuadDestroy += OnQuadDestroyed;
            quadsHandler.OutsidePlayingArea += OnOutsidePlayingArea;
        }

        public void PrepareUI()
        {
            localizationSetuper.SetCurrentLocalization(Localization.Ru);
            CreateButtonsQuad();
            messageShower.StartMessage();
        }

        private void CreateButtonsQuad()
        {
            quadsHandler.CreateQuadSockets();
            scrollBarHandler.FillQuadButtons(quadsHandler.QuadSocketObjects);
        }

        private void OnQuadBuilded()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadBuildMessage);
        }

        private void OnQuadDestroyed()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadDestroyMessage);
        }
        
        private void OnOutsidePlayingArea()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().OutsidePlayingAreMessage);
        }
    }
}