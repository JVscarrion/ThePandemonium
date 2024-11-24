using UnityEngine;
using UnityEngine.Tilemaps;

public class FitGridToCamera : MonoBehaviour
{
    public Tilemap tilemap; 
    public Camera mainCamera;

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

       
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect * 1.1f;

       
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int size = bounds.size;
        float tilemapAspect = (float)size.x / size.y;

       
        float desiredScaleX = camWidth / size.x;
        float desiredScaleY = camHeight / size.y;

        tilemap.transform.localScale = new Vector3(desiredScaleX, desiredScaleY, 1f);
    }
}
