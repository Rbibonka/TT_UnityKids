using UnityEngine;

namespace Utils
{
    public static class UIObjectInsideCheck
    {
        public static bool IsFullyInside(RectTransform parent, RectTransform child)
        {
            Vector3[] parentCorners = new Vector3[4];
            Vector3[] childCorners = new Vector3[4];

            parent.GetWorldCorners(parentCorners);
            child.GetWorldCorners(childCorners);

            Rect parentRect = new Rect(
                parentCorners[0].x,
                parentCorners[0].y,
                parentCorners[2].x - parentCorners[0].x,
                parentCorners[2].y - parentCorners[0].y);

            foreach (Vector3 corner in childCorners)
            {
                if (!parentRect.Contains(new Vector2(corner.x, corner.y)))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsInside(RectTransform rectTransformA, RectTransform rectTransformB)
        {
            Rect GetWorldRect(RectTransform rectTransform)
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                float minX = corners[0].x;
                float maxX = corners[2].x;
                float minY = corners[0].y;
                float maxY = corners[1].y;

                return new Rect(minX, minY, maxX - minX, maxY - minY);
            }

            Rect rectA = GetWorldRect(rectTransformA);
            Rect rectB = GetWorldRect(rectTransformB);
            return rectA.Overlaps(rectB);
        }

        public static bool IsEllipseTouchingRectangle(RectTransform ellipseRect, RectTransform rect)
        {
            Vector2 ellipseCenter = GetWorldPosition2D(ellipseRect);

            Vector2 ellipseSize = ellipseRect.rect.size;
            Vector3 ellipseScale = ellipseRect.lossyScale;
            float radiusX = (ellipseSize.x * ellipseScale.x) * 0.5f;
            float radiusY = (ellipseSize.y * ellipseScale.y) * 0.5f;

            Vector2 rectCenter = GetWorldPosition2D(rect);
            Vector2 rectSize = new Vector2(rect.rect.width * rect.lossyScale.x, rect.rect.height * rect.lossyScale.y);

            Vector2 rectMin = rectCenter - rectSize * rect.pivot;
            Vector2 rectMax = rectMin + rectSize;

            float closestX = Mathf.Clamp(ellipseCenter.x, rectMin.x, rectMax.x);
            float closestY = Mathf.Clamp(ellipseCenter.y, rectMin.y, rectMax.y);

            Vector2 closestPoint = new Vector2(closestX, closestY);
            Vector2 delta = closestPoint - ellipseCenter;

            float value = (delta.x * delta.x) / (radiusX * radiusX) + (delta.y * delta.y) / (radiusY * radiusY);
            return value <= 1f;
        }

        private static Vector2 GetWorldPosition2D(RectTransform rectTransform)
        {
            Vector3 pos = rectTransform.position;
            return new Vector2(pos.x, pos.y);
        }
    }
}