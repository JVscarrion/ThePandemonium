using UnityEngine;
using UnityEngine.Tilemaps;

public class FitGridToCamera : MonoBehaviour
{
    public Tilemap tilemap; // Reference to your Tilemap component
    public Camera mainCamera; // Reference to your Camera component

    void Start()
    {
        FitTilemapToCamera();
    }

    private void Update()
    {
        FitTilemapToCamera();
    }

    void FitTilemapToCamera()
    {
        if (tilemap == null || mainCamera == null)
        {
            Debug.LogError("Tilemap or Camera references not set!");
            return;
        }

        // Get the size of the camera's viewport in world units
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect * 1.1f;

        // Calculate the aspect ratio of the tilemap
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int size = bounds.size;
        float tilemapAspect = (float)size.x / size.y;

        // Adjust the scale of the tilemap to fit the camera's aspect ratio
        float desiredScaleX = camWidth / size.x;
        float desiredScaleY = camHeight / size.y;

        tilemap.transform.localScale = new Vector3(desiredScaleX, desiredScaleY, 1f);
    }
}
