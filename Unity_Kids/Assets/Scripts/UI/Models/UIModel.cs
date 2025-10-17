using UnityEngine;
using Handlers;
using Configs;
using Objects;

namespace Models
{
    public sealed class UIModel : MonoBehaviour
    {
        [SerializeField]
        private ScrollBarModel scrollBarModel;

        [SerializeField]
        private Canvas canvas;

        private UIHandler uiHandler;

        private ScrollBarHandler scrollBarHandler;

        public UIHandler GetHandler(QuadConfig[] quads, QuadObject quadButtonObject, QuadSocketObject quadSocketObject)
        {
            if (uiHandler == null || !uiHandler.IsInitilized)
            {
                scrollBarHandler = scrollBarModel.GetHandler();

                uiHandler = new();
                uiHandler.Initialize(quadButtonObject, quadSocketObject, scrollBarHandler, quads, canvas);
            }

            return uiHandler;
        }
    }
}