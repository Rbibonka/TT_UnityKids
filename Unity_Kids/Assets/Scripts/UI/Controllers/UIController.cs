using Configs;
using Controller;
using Cysharp.Threading.Tasks;
using Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public sealed class UIController : IDisposable
    {
        private ScrollBarController scrollBarHandler;
        private QuadsController quadsController;
        private LocalizationSetuper localizationSetuper;
        private QuadSocketsController socketsController;
        private MessageShowContoller messageShower;
        private QuadsLifeCycleConrtoller quadsLifeCycleController;
        private QuadsSaveController quadsSaveController;
        private CancellationTokenSource cts;

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
            GarbageCollectorObject garbageCollectorObject,
            List<TowerQuad> savedQuads)
        {
            this.scrollBarHandler = scrollBarHandler;
            this.localizationSetuper = localizationSetuper;

            quadsLifeCycleController = new(quads, quadObject, releasedQuadsParent, canvas);
            quadsSaveController = new(quadsLifeCycleController, savedQuads);
            quadsController = new(releasedQuadsParent, playZone, towerHead, canvasRectTransform, garbageCollectorObject);
            messageShower = new(txt_Message);

            quadsSaveController.BeginDragged += OnQuadSocketReleased;

            socketsController = new(quads, quadSocketObject, quadsLifeCycleController);
            socketsController.QuadReleased += OnQuadSocketReleased;
            socketsController.SocketEmpty += OnSocketEmpty;

            quadsController.MessageQuadBuilded += OnMessageQuadBuilded;
            quadsController.MessageQuadDestroyed += OnMessageQuadDestroyed;
            quadsController.MessageOutsidePlayingAreaWent += OnMessageOutsidePlayingAreaWent;

            quadsController.QuadDestroyed += OnQuadDestroyed;
        }

        public void PrepareUI()
        {
            localizationSetuper.SetCurrentLocalization(Localization.Ru);

            var quadSockets = socketsController.CreateQuadSockets();

            scrollBarHandler.FillQuadButtons(quadSockets);

            var quads = quadsSaveController.CreateSavedQuads();

            if (quads.Count > 0)
            {
                quadsController.AddSavedQuads(quads);
            }

            cts = new();

            messageShower.ShowAnimationTextLoop(cts.Token).Forget();
        }

        private void OnSocketEmpty(int socketId)
        {
            socketsController.SetQuadToSocket(socketId);
        }

        private void OnQuadDestroyed(QuadObject quad)
        {
            quadsLifeCycleController.SetQuad(quad);
        }

        private void OnQuadSocketReleased(QuadObject quad)
        {
            quadsController.SetCurrentQuad(quad);
        }

        private void OnMessageQuadBuilded()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadBuildMessage);
        }

        private void OnMessageQuadDestroyed()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadDestroyMessage);
        }

        private void OnMessageOutsidePlayingAreaWent()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().OutsidePlayingAreMessage);
        }

        public List<TowerQuad> GetTowerQuads()
        {
            return quadsController.GetTowerQuads();
        }

        public void Dispose()
        {
            socketsController.QuadReleased -= OnQuadSocketReleased;
            socketsController.SocketEmpty -= OnSocketEmpty;
            socketsController.Dispose();

            quadsController.MessageQuadBuilded -= OnMessageQuadBuilded;
            quadsController.QuadDestroyed -= OnQuadDestroyed;
            quadsController.MessageOutsidePlayingAreaWent -= OnMessageOutsidePlayingAreaWent;
            quadsController.Dispose();

            quadsSaveController.BeginDragged -= OnQuadSocketReleased;
            quadsSaveController.Dispose();


            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }
    }
}