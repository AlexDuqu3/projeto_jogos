using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    public float TileSize { get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; } }
    [SerializeField]
    private CameraMoviment cameraMoviment;
    private GameObject map;
    private Vector2 mapPosition;
    public Vector2 MapPosition
    {
        get { return mapPosition; }
        private set { mapPosition = value; }
    }
    private Point nexusPoint;
    public Point NexusPoint
    {
        get { return nexusPoint; }
        private set { nexusPoint = value; }
    }
    [SerializeField]
    private GameObject nexusPrefab;
    public Dictionary<Point, Tile> Tiles { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, Tile>();
        map = new GameObject("Map");
        string[] mapData = ReadLevelText();
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;
        MapPosition= new Vector2(mapX, mapY);
        Vector3 maxTile = Vector3.zero;
        Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
             PlaceTile(newTiles[x].ToString(), x, y, worldStartPosition);
            }
        }
        Vector2 worldPositionNexus = SpawnNexus();
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMoviment.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        cameraMoviment.setCamaraPosition(new Vector3(worldPositionNexus.x, worldPositionNexus.y, -10));
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStartPosition)
    {
        int tileIndex = int.Parse(tileType);
        Tile newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<Tile>();
        newTile.Setup(new Point(x, y), new Vector3(worldStartPosition.x + (TileSize * x), worldStartPosition.y - (TileSize * y), 0), map.transform);
    }


    private string[] ReadLevelText()
    {
        TextAsset data = Resources.Load("Level") as TextAsset;
        string dataString = data.text.Replace(Environment.NewLine, string.Empty);
        return dataString.Split('-');
    }

    private Vector2 SpawnNexus()
    {
        int mapX = (int)MapPosition.x;
        int mapY = (int)MapPosition.y;
        int halfWidth = mapX / 2;
        int halfHeight = mapY / 2;

        int minNexusPosX = halfWidth / 2 + 1;
        int maxNexusPosX = halfWidth + halfWidth / 2;

        int minNexusPosY = halfHeight / 2 + 1;
        int maxNexusPosY = halfHeight + halfHeight / 2;

        int nexusPosX = UnityEngine.Random.Range(minNexusPosX, maxNexusPosX + 1);
        int nexusPosY = UnityEngine.Random.Range(minNexusPosY, maxNexusPosY + 1);
        NexusPoint= new Point(nexusPosX, nexusPosY);
        Tile tile = Tiles[NexusPoint];
        Vector2 worldPositionNexus = tile.PlaceNexus(nexusPrefab);
        return worldPositionNexus;

    }

    public Tile GetTileAtWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);

        if (hitCollider != null)
        {
            Tile tile = hitCollider.GetComponent<Tile>();
            if (tile != null)
            {
                return tile;
            }
        }
        // Tile not found, return null or handle the case accordingly
        return null;
    }

    public Vector2 GetRandomPointOutsideMap(int radius)
    {
        Vector3 maxTile = Tiles[new Point((int)MapPosition.x - 1, (int)MapPosition.y - 1)].transform.position;
        Vector3 mapSize = new Vector3(maxTile.x + TileSize, maxTile.y - TileSize);
        int mapWidth = (int)mapSize.x;
        int mapHeight = (int)mapSize.y;
        int minX = -radius;
        int maxX = mapWidth + radius;
        int minY = -radius;
        int maxY = mapHeight + radius;

        List<Vector2> eligiblePoints = new List<Vector2>();
        // Collect eligible points
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Vector2 point = new Vector2(x, y);

                // Check if the point is outside the map, but within the radius
                if (Vector2.Distance(point, new Vector2(mapWidth / 2, mapHeight / 2)) > (mapWidth / 2) + radius)
                {
                    eligiblePoints.Add(point);
                }
            }
        }

        // Return a random point from the eligible points
        if (eligiblePoints.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, eligiblePoints.Count);
            return eligiblePoints[randomIndex];
        }
        else
        {
            // Return a default point if no eligible points are found
            return Vector2.zero;
        }
    }
}
