using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public sealed class MessageShowContoller
    {
        private TMP_Text text;

        private string currentMessage;

        private bool isNewMessageReady;
        private CancellationTokenSource cts;

        public MessageShowContoller(TMP_Text text)
        {
            this.text = text;
        }

        public void StartMessage()
        {
            cts = new();

            AnimationText(cts.Token).Forget();
        }

        public void Message(string message)
        {
            currentMessage = message;

            isNewMessageReady = true;
        }

        private async UniTask AnimationText(CancellationToken ct)
        {
            text.transform.localScale = Vector3.zero;

            while (true)
            {
                await UniTask.WaitUntil(() => isNewMessageReady);

                text.text = currentMessage;

                isNewMessageReady = false;

                await UniTask.WhenAll(text.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutBounce).WithCancellation(ct));

                await UniTask.WaitForSeconds(1f);

                await UniTask.WhenAll(text.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBounce).WithCancellation(ct));
            }
        }
    }
}