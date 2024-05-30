using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public static ShipMovement Instance;
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 originalPosition;
    public int length;
    public int height;
    public int gridSize = 7;
    private bool isOccupied;
    public int selectedShip;

    private void Awake()
    {
        Instance = this;
    }

    void OnMouseDown()
    {
        Gamemanager.Instance.shipIndex = selectedShip;
;       offset = transform.position - GetMouseWorldPos();
        isDragging = true;
        originalPosition = transform.position;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        Vector2 nearestGridPosition = GetNearestGridPosition(transform.position);
        if (isOccupied)
        {
            transform.position = originalPosition;
        }
        else
        {
            transform.position = nearestGridPosition;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private Vector2 GetNearestGridPosition(Vector3 position)
    {
        int gridX = Mathf.RoundToInt(position.x);
        int gridY = Mathf.RoundToInt(position.y);
        gridX = Mathf.Clamp(gridX, 0, gridSize -1);
        gridY = Mathf.Clamp(gridY, 0, gridSize-1);
        return new Vector2(gridX, gridY);
    }

    private void EnableSpriteRenderer(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grid"))
        {
            Tiles tile = collision.GetComponent<Tiles>();
            Gamemanager.Instance.occupiedPos.Add(new Vector2(tile.posX, tile.posY));
        }
        if (collision.CompareTag("Ship"))
        {
            isOccupied = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grid"))
        {
            Tiles tile = collision.GetComponent<Tiles>();
            Gamemanager.Instance.occupiedPos.Remove(new Vector2(tile.posX, tile.posY));
        }
        if (collision.CompareTag("Ship"))
        {
            isOccupied = false;
        }
    }
}

