using UnityEngine;
using Controllers;
using Configs;
using Objects;
using TMPro;
using Controller;
using System.Collections.Generic;

namespace Models
{
    public sealed class UIModel : MonoBehaviour
    {
        [SerializeField]
        private ScrollBarModel scrollBarModel;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Transform releasedQuadsParent;

        [SerializeField]
        private RectTransform playZone;

        [SerializeField]
        private TowerHead towerHead;

        [SerializeField]
        private RectTransform canvasRectTransform;

        [SerializeField]
        private TMP_Text txt_Message;

        [SerializeField]
        private GarbageCollectorObject garbageCollectorObject;

        private UIController uiController;

        private ScrollBarController scrollBarHandler;

        public UIController GetHandler(QuadConfig[] quads, QuadObject quadButtonObject, QuadSocketObject quadSocketObject, LocalizationSetuper localizationSetuper, List<TowerQuad> savedQuads)
        {
            if (uiController == null || !uiController.IsInitilized)
            {
                scrollBarHandler = scrollBarModel.GetHandler();

                uiController = new(quadButtonObject, quadSocketObject, scrollBarHandler, quads, canvas, releasedQuadsParent,
                    playZone, towerHead, canvasRectTransform, txt_Message, localizationSetuper, garbageCollectorObject, savedQuads);
            }

            return uiController;
        }
    }
}