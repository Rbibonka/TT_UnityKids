using System;
using UnityEngine;

namespace Objects
{
    public class QuadSocketObject : MonoBehaviour
    {
        private int quadSocketId;
        private Sprite quadSprite;
        private Canvas canvas;
        private QuadObject quadObject;

        public event Action<int> SocketRelease;

        public void Initialize(int quadSocketId, Sprite quadSprite, Canvas canvas, QuadObject quadObject)
        {
            this.quadSocketId = quadSocketId;
            this.quadSprite = quadSprite;
            this.canvas = canvas;
            this.quadObject = quadObject;
        }

        public void CreateQuad()
        {
            var newQuadObject = Instantiate(quadObject, transform);
            newQuadObject.Initialize(quadSocketId, canvas);
            newQuadObject.SetSprite(quadSprite);
        }
    }
}