using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public Point GridPosition { get; private set; }
    private bool isEmpty;
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);
    private Color32 disabledColor = new Color32(72, 72, 72, 255);
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
    private bool isNexusArea;
    public bool IsNexusArea
    {
        get
        {
            return isNexusArea;
        }
       private set
        {
            isNexusArea = value;
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
    public Vector2 WorldPosition
    {
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
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void Setup(Point gridPosition, Vector3 worldPosition, Transform parent)
    {
        NumberOftowers = 0;
        IsEmpty = true;
        isNexusArea= false;
        this.GridPosition = gridPosition;
        IsDisable = IsBorderTile();
        transform.position = worldPosition;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPosition, this);
    }
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManage.Instance.TowerPlacementBtn != null)
        {
            Point[] adjacentPoints = GetAdjacentPoints(GameManage.Instance.TowersAdjacentRadius); //3x3
            if (IsDisable)
            {
                ColorTile(disabledColor);
                ColorAdjacentTiles(disabledColor, adjacentPoints);
            }
            else
            {
                ColorTile(isEmpty ? emptyColor : fullColor);
                ColorAdjacentTiles(isEmpty ? emptyColor : fullColor, adjacentPoints);
            }
            if (Input.GetMouseButtonDown(1) && IsEmpty && !IsDisable)
            {
                PlaceTower();
            }
        }

    }
    private void OnMouseExit()
    {
        Point[] adjacentPoints = GetAdjacentPoints(GameManage.Instance.TowersAdjacentRadius);
        ColorTile(Color.white);
        ColorAdjacentTiles(Color.white, adjacentPoints);
    }


    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManage.Instance.TowerPlacementBtn.TowerPrefab, WorldPosition, Quaternion.identity);
        // tower.AddComponent<Tower>();
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);
        Hover.Instance.Deactivate();
        tower.GetComponent<Tower>().Price = GameManage.Instance.TowerPlacementBtn.Price;
        GameManage.Instance.BuyTower();
        IsEmpty = false;
        NumberOftowers++;
        MarkAdjacentPointsDurty(GetAdjacentPoints(GameManage.Instance.TowersAdjacentRadius));
        ColorTile(Color.white);
    }
    public Vector2 PlaceNexus(GameObject nexusPrefab)
    {
        GameObject nexus = Instantiate(nexusPrefab, WorldPosition, Quaternion.identity);
        nexus.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        nexus.transform.SetParent(transform);
        Point[] adjacentPoints = GetAdjacentPoints(5);
        for(int i = 0; i < adjacentPoints.Length; i++)
        {
            if (LevelManager.Instance.Tiles.ContainsKey(adjacentPoints[i]))
            {
                if (IsTileWithinBounds())
                {
                    Tile adjacentTile = LevelManager.Instance.Tiles[adjacentPoints[i]];
                    adjacentTile.IsDisable = true;
                    adjacentTile.isNexusArea = true;
                }
                
            }
        }
        return WorldPosition;

    }

    public Vector2 PlaceDecoration(GameObject decorationPrefab)
    {
        GameObject decoration = Instantiate(decorationPrefab, WorldPosition, Quaternion.identity);
        if (decoration.transform.Find("Upper"))
        {
            decoration.transform.Find("Upper").GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            decoration.transform.Find("Lower").GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
            decoration.transform.Find("Shadow").GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        }
        decoration.transform.SetParent(transform);
        Point[] adjacentPoints = GetAdjacentPoints(1);
        for (int i = 0; i < adjacentPoints.Length; i++)
        {
            if (LevelManager.Instance.Tiles.ContainsKey(adjacentPoints[i]))
            {
                if (IsTileWithinBounds())
                {
                    Tile adjacentTile = LevelManager.Instance.Tiles[adjacentPoints[i]];
                    adjacentTile.IsDisable = true;
                    adjacentTile.isNexusArea = true;
                }

            }
        }
        return WorldPosition;

    }

    public void ColorTile(Color newColor)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = newColor;
    }

    public void ColorAdjacentTiles(Color newColor, Point[] adjacentPoints)
    {
        if(LevelManager.Instance.Tiles==null)
        {
            return;
        }
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

    //private Point[] GetAdjacentPoints()
    //{
    //    int[] xOffset = { -1, 0, 1, 0, -1, 1, -1, 1 };
    //    int[] yOffset = { 0, 1, 0, -1, -1, 1, 1, -1 };
    //    Point centerPoint = GridPosition;
    //    Point[] adjacentPoints = new Point[8];

    //    for (int i = 0; i < 8; i++)
    //    {
    //        int adjacentX = centerPoint.X + xOffset[i];
    //        int adjacentY = centerPoint.Y + yOffset[i];
    //        Point adjacentPoint = new Point(adjacentX, adjacentY);
    //        adjacentPoints[i] = adjacentPoint;
    //    }

    //    return adjacentPoints;
    //}
    public Point[] GetAdjacentPoints(int adjacentRadius)
    {
        int size = (2 * adjacentRadius + 1) * (2 * adjacentRadius + 1);
        Point centerPoint = GridPosition;
        Point[] adjacentPoints = new Point[size];

        int index = 0;

        for (int y = -adjacentRadius; y <= adjacentRadius; y++)
        {
            for (int x = -adjacentRadius; x <= adjacentRadius; x++)
            {
                int adjacentX = centerPoint.X + x;
                int adjacentY = centerPoint.Y + y;
                Point adjacentPoint = new Point(adjacentX, adjacentY);
                adjacentPoints[index] = adjacentPoint;
                index++;
            }
        }

        return adjacentPoints;
    }

    private void MarkAdjacentPointsDurty(Point[] adjacentPoints)
    {
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

    public void MarkAdjacentPointsPristine(Point[] adjacentPoints)
    {
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
    private bool IsBorderTile()
    {
        int x=GridPosition.X;
        int y=GridPosition.Y;
        // Check if the tile is on the border
        return x == 0 || y == 0 || x == (int)LevelManager.Instance.MapPosition.x - 1 || y == (int)LevelManager.Instance.MapPosition.y - 1;
    }

    private bool IsTileWithinBounds()
    {
        int x=GridPosition.X;
        int y=GridPosition.Y;
        return x >= 0 && x < (int)LevelManager.Instance.MapPosition.x && y >= 0 && y < (int)LevelManager.Instance.MapPosition.y;
    }

}