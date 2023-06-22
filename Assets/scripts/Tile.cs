using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Point GridPosition { get; private set; }
    private bool isEmpty;
    private Color32 fullColor= new Color32(255, 118, 118, 255);
    private Color32 emptyColor= new Color32(96, 255, 90, 255);
    private Color32 disabledColor= new Color32(72, 72, 72, 255);
    private SpriteRenderer spriteRenderer;
    private int numberOftowers;
    private bool isDisable;
    public bool IsDisable
    {
        get
        {
            return isDisable;
        }
        set
        {
            isDisable = value;
        }
    }

    public int NumberOftowers
    {
        get
        {
            return numberOftowers;
        }
        set
        {
            numberOftowers = value;
        }
    }
    public bool IsEmpty
    {
        get
        {
            return isEmpty;
        }
        set
        {
            isEmpty = value;
        }
    }   
    public Vector2 WorldPosition {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));   //Get tile center
        }
    }

    private void Awake()
    {
        IsDisable = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Point gridPosition,Vector3 worldPosition,Transform parent)
    {
        NumberOftowers = 0;
        IsEmpty = true;
        this.GridPosition = gridPosition;
        transform.position = worldPosition;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPosition, this);
    }
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManage.Instance.TowerPlacementBtn!=null)
        {
            if(IsDisable)
            {
                ColorTile(disabledColor);
            }
            else
            {
                ColorTile(isEmpty ? emptyColor : fullColor);
                ColorAdjacentTiles(isEmpty ? emptyColor : fullColor);
            }
            if (Input.GetMouseButtonDown(1) && IsEmpty && !IsDisable)
            {
                PlaceTower();
            }
        }
        
    }
    private void OnMouseExit()
    {
        ColorTile(Color.white);
        ColorAdjacentTiles(Color.white);
    }
    

    private void PlaceTower()
    {
        GameObject tower=Instantiate(GameManage.Instance.TowerPlacementBtn.TowerPrefab, WorldPosition, Quaternion.identity);
       // tower.AddComponent<Tower>();
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);
        Hover.Instance.Deactivate();
        GameManage.Instance.BuyTower();
        IsEmpty = false;
        NumberOftowers++;
        MarkAdjacentPointsDurty();
        ColorTile(Color.white);
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    private void ColorAdjacentTiles(Color newColor)
    {
        Point[] adjacentPoints = GetAdjacentPoints();
        foreach (Point adjacentPoint in adjacentPoints)
        {
            // Verificar se o tile adjacente existe no dicionário
            if (LevelManager.Instance.Tiles.ContainsKey(adjacentPoint))
            {
                Tile adjacentTile = LevelManager.Instance.Tiles[adjacentPoint];  
                adjacentTile.ColorTile(newColor);
            }
        }
    }

    private Point[] GetAdjacentPoints()
    {
        int[] xOffset = { -1, 0, 1, 0, -1, 1, -1, 1 };
        int[] yOffset = { 0, 1, 0, -1, -1, 1, 1, -1 };
        Point centerPoint = GridPosition;
        Point[] adjacentPoints = new Point[8];

        for (int i = 0; i < 8; i++)
        {
            int adjacentX = centerPoint.X + xOffset[i];
            int adjacentY = centerPoint.Y + yOffset[i];
            Point adjacentPoint = new Point(adjacentX, adjacentY);
            adjacentPoints[i] = adjacentPoint;
        }

        return adjacentPoints;
    }

    private void MarkAdjacentPointsDurty()
    {
        Point[] adjacentPoints = GetAdjacentPoints();
        foreach (Point adjacentPoint in adjacentPoints)
        {
            // Verificar se o tile adjacente existe no dicionário
            if (LevelManager.Instance.Tiles.ContainsKey(adjacentPoint))
            {
                Tile adjacentTile = LevelManager.Instance.Tiles[adjacentPoint];
                adjacentTile.IsEmpty = false;
                adjacentTile.NumberOftowers++;
                adjacentTile.ColorTile(Color.white);
            }
        }
    }

    public void MarkAdjacentPointsPristine()
    {
        Point[] adjacentPoints = GetAdjacentPoints();
        foreach (Point adjacentPoint in adjacentPoints)
        {
            // Verificar se o tile adjacente existe no dicionário
            if (LevelManager.Instance.Tiles.ContainsKey(adjacentPoint))
            {
                Tile adjacentTile = LevelManager.Instance.Tiles[adjacentPoint];
                if (adjacentTile.numberOftowers == 0)
                {
                    return;
                }
                if (adjacentTile.NumberOftowers == 1)
                {
                    adjacentTile.IsEmpty = true;
                }
                    adjacentTile.NumberOftowers--;
            }
        }
    }
}