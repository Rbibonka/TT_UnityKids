using Configs;
using Controller;
using Objects;
using System;
using System.Collections.Generic;
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

        private QuadsObjectPool quadsObjectPool;
        private QuadConfig[] quads;

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
            this.quads = quads;

            quadsObjectPool = new(quadObject, releasedQuadsParent);

            quadsController = new(releasedQuadsParent, playZone, towerHead, canvasRectTransform, garbageCollectorObject, savedQuads);

            messageShower = new(txt_Message);

            socketsController = new(quads, quadSocketObject, quadObject, canvas);
            socketsController.QuadReleased += OnQuadSocketReleased;
            socketsController.SocketEmpty += OnSocketEmpty;

            quadsController.QuadBuilded += OnQuadBuilded;
            quadsController.QuadDestroyed += OnQuadDestroyed;
            quadsController.OutsidePlayingAreaWent += OnOutsidePlayingAreaWent;
        }

        public void PrepareUI()
        {
            localizationSetuper.SetCurrentLocalization(Localization.Ru);

            var quadSockets = socketsController.CreateQuadSockets();

            QuadObject[] socketQuads = new QuadObject[quads.Length];

            for(int i = 0; i < quads.Length; i++)
            {
                socketQuads[i] = quadsObjectPool.GetFromPool();
                socketsController.SetQuadToSocket(i, socketQuads[i]);
            }

            scrollBarHandler.FillQuadButtons(quadSockets);

            messageShower.StartMessage();
        }

        private void OnSocketEmpty(int socketId)
        {
            var quad = quadsObjectPool.GetFromPool();

            socketsController.SetQuadToSocket(socketId, quad);
        }

        private void OnQuadDestroyed(QuadObject quad)
        {
            quadsObjectPool.SetToPool(quad);

            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadDestroyMessage);
        }

        private void OnQuadSocketReleased(QuadObject quad)
        {
            quadsController.SetCurrentQuad(quad);
        }

        private void OnQuadBuilded()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().QuadBuildMessage);
        }
        
        private void OnOutsidePlayingAreaWent()
        {
            messageShower.Message(localizationSetuper.GetCurrentLocalizationConfig().OutsidePlayingAreMessage);
        }

        public void Dispose()
        {
            socketsController.QuadReleased -= OnQuadSocketReleased;
            socketsController.SocketEmpty -= OnSocketEmpty;

            quadsController.QuadBuilded -= OnQuadBuilded;
            quadsController.QuadDestroyed -= OnQuadDestroyed;
            quadsController.OutsidePlayingAreaWent -= OnOutsidePlayingAreaWent;

            quadsController.Dispose();
        }
    }
}