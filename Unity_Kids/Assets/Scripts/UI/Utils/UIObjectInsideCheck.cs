using UnityEngine;

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
}