using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
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
        SpawnNexus();
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMoviment.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
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

    private void SpawnNexus()
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
        nexusPoint= new Point(nexusPosX, nexusPosY);
        Tile tile = Tiles[nexusPoint];
        tile.PlaceNexus(nexusPrefab);
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
}
