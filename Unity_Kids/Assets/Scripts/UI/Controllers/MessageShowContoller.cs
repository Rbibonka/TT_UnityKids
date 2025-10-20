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

        public MessageShowContoller(TMP_Text text)
        {
            this.text = text;
        }

        public void Message(string message)
        {
            currentMessage = message;

            isNewMessageReady = true;
        }

        public async UniTask ShowAnimationTextLoop(CancellationToken ct)
        {
            text.transform.localScale = Vector3.zero;

            while (!ct.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => isNewMessageReady);

                text.text = currentMessage;

                isNewMessageReady = false;

                await UniTask.WhenAll(text.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutBounce).WithCancellation(ct));

                await UniTask.WaitForSeconds(0.6f);

                await UniTask.WhenAll(text.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBounce).WithCancellation(ct));
            }
        }
    }
}