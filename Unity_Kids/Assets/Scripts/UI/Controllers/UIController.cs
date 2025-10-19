using Configs;
using Controller;
using Objects;
using System;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public sealed class UIController : IDisposable
    {
        private ScrollBarController scrollBarHandler;

        private QuadsController quadsController;
        private LocalizationSetuper localizationSetuper;

        private QuadSocketsController socketsHandler;

        private MessageShowContoller messageShower;

        public bool IsInitilized { get; private set; }

        public UIController(
            QuadObject quadObject,
            QuadSocketObject quadSocketObject,
            ScrollBarController scrollBarHandler,
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

            quadsController = new(releasedQuadsParent, playZone, towerHead, canvasRectTransform, garbageCollectorObject);

            messageShower = new(txt_Message);

            socketsHandler = new(quads, quadSocketObject, quadObject, canvas);
            socketsHandler.QuadReleased += OnQuadSocketReleased;

            quadsController.QuadBuilded += OnQuadBuilded;
            quadsController.QuadDestroyed += OnQuadDestroyed;
            quadsController.OutsidePlayingAreaWent += OnOutsidePlayingAreaWent;
        }

        public void PrepareUI()
        {
            localizationSetuper.SetCurrentLocalization(Localization.Ru);

            var quadSockets = socketsHandler.CreateQuadSockets();
            scrollBarHandler.FillQuadButtons(quadSockets);

            messageShower.StartMessage();
        }

        private void OnQuadSocketReleased(QuadObject quad)
        {
            quadsController.SetCurrentQuad(quad);
        }

        private void OnQuadBuilded()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadBuildMessage);
        }

        private void OnQuadDestroyed()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadDestroyMessage);
        }
        
        private void OnOutsidePlayingAreaWent()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().OutsidePlayingAreMessage);
        }

        public void Dispose()
        {
            quadsController.QuadBuilded -= OnQuadBuilded;
            quadsController.QuadDestroyed -= OnQuadDestroyed;
            quadsController.OutsidePlayingAreaWent -= OnOutsidePlayingAreaWent;

            quadsController.Dispose();
        }
    }
}