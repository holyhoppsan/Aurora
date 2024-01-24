using UnityEngine;

public static class DebugRendering
{
    public static void DrawBoundingBox(Vector3 minCorner, Vector3 maxCorner, Color color)
    {
        Vector3[] corners = new Vector3[8];

        // Calculate the eight corners of the bounding box
        corners[0] = new Vector3(minCorner.x, minCorner.y, minCorner.z);
        corners[1] = new Vector3(minCorner.x, minCorner.y, maxCorner.z);
        corners[2] = new Vector3(maxCorner.x, minCorner.y, maxCorner.z);
        corners[3] = new Vector3(maxCorner.x, minCorner.y, minCorner.z);
        corners[4] = new Vector3(minCorner.x, maxCorner.y, minCorner.z);
        corners[5] = new Vector3(minCorner.x, maxCorner.y, maxCorner.z);
        corners[6] = new Vector3(maxCorner.x, maxCorner.y, maxCorner.z);
        corners[7] = new Vector3(maxCorner.x, maxCorner.y, minCorner.z);

        // Draw lines between the corners to create the bounding box
        for (int i = 0; i < 4; i++)
        {
            int nextIndex = (i + 1) % 4;
            Debug.DrawLine(corners[i], corners[nextIndex], color);
            Debug.DrawLine(corners[i + 4], corners[nextIndex + 4], color);
            Debug.DrawLine(corners[i], corners[i + 4], color);
        }

        // Connect the top and bottom corners
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 4], color);
        }
    }

    public static void RenderScreenSpaceDebugLine(Camera camera, Vector2 screenStartPoint, Vector2 screenEndPoint, Color color)
    {
        // Convert screen space coordinates to world space
        Vector3 worldStartPoint = camera.ScreenToWorldPoint(new Vector3(screenStartPoint.x, screenStartPoint.y, camera.nearClipPlane));
        Vector3 worldEndPoint = camera.ScreenToWorldPoint(new Vector3(screenEndPoint.x, screenEndPoint.y, camera.nearClipPlane));

        // Draw the debug line in world space
        Debug.DrawLine(worldStartPoint, worldEndPoint, color);
    }
}
