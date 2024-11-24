using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    public float gridMovementSize;
   
    public GameObject spawnedPrefab;
    private bool isDragging;

    private SpriteRenderer towerSprite1;
    private SpriteRenderer towerSprite2;
    private SpriteRenderer towerSprite3;
    void Update()
    {
        if (isDragging && spawnedPrefab != null)
        {
            HandleDrag(spawnedPrefab);
        }
    }

    public void DragStart(GameObject prefab)
    {
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
        }
        if (prefab != null)
        {
           // if (!EventSystem.current.IsPointerOverGameObject())
           // {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = SnapToGrid(mousePos); 
                spawnedPrefab = Instantiate(prefab, mousePos, prefab.transform.rotation);
                isDragging = true;
           // }
        }
        
    }

    private void HandleDrag(GameObject prefab)
    {
        if (prefab != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = SnapToGrid(mousePos); 
                prefab.transform.position = mousePos;
                //
                if (IsValidPosition(mousePos))
                {
                    towerSprite1 = spawnedPrefab.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite2 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite3 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

                    towerSprite1.color = Color.red;
                    towerSprite2.color = Color.red;
                    towerSprite3.color = Color.red;
                }
                else if (IsGround(mousePos))
                {
                    
                    towerSprite1 = spawnedPrefab.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite2 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite3 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

                    towerSprite1.color = Color.green;
                    towerSprite2.color = Color.green;
                    towerSprite3.color = Color.green;
                }
                else
                {
                    towerSprite1 = spawnedPrefab.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite2 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite3 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

                    towerSprite1.color = Color.red;
                    towerSprite2.color = Color.red;
                    towerSprite3.color = Color.red;
                }
                //
                if (Input.GetMouseButtonUp(0))
                {
                    DragEnd();
                }
            }
        }
    }

    public void DragEnd()
    {
        if (spawnedPrefab != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = SnapToGrid(mousePos); 
                if (IsValidPosition(mousePos))
                {
                    Destroy(spawnedPrefab); 
                }
                else if(IsGround(mousePos))
                {
                    spawnedPrefab.layer = LayerMask.NameToLayer("Tower");


                    towerSprite1 = spawnedPrefab.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite2 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    towerSprite3 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

                    towerSprite1.color = Color.white;
                    towerSprite2.color = Color.white;
                    towerSprite3.color = Color.white;
                }
                else 
                {
                    Destroy(spawnedPrefab);
                }
                isDragging = false;
            }
            else
            {
                Destroy(spawnedPrefab);
            }
        }
    }
    private Vector2 SnapToGrid(Vector2 position)
    {
        
        float gridSize = gridMovementSize;

        
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;

       
        Vector2 snappedPosition = new Vector2(snappedX, snappedY);

        return snappedPosition;
    }

    private bool IsValidPosition(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f); 
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
            {
                return true; 
            }
        }
        return false; 
    }
    private bool IsGround(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.25f); 
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true; 
            }
        }
        return false; 
    }
}
