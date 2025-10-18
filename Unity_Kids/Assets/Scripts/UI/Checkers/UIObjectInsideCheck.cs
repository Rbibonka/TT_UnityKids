using UnityEngine;

public class UIObjectInsideCheck
{
    public bool IsFullyInside(RectTransform parent, RectTransform child)
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
}