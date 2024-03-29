using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    private GameObject[] decorations;
    public float TileSize { get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; } }
    [SerializeField]
    private CameraMoviment cameraMoviment;
    private GameObject map;
    private Vector3 worldMapPositionInitial;
    public Vector3 WorldMapPositionInitial { get => worldMapPositionInitial; set => worldMapPositionInitial = value; }
    private Vector3 maxTile;
    public Vector3 MaxTile { get => maxTile; private set => maxTile = value; }
    private List<Vector2> spawnPositions;
    public List<Vector2> SpawnPositions { get => spawnPositions; private set => spawnPositions = value; }
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
    private RandomPointsGenerator randomPointsGenerator;
    public RandomPointsGenerator RandomPointsGenerator { get => randomPointsGenerator; set => randomPointsGenerator = value; }

    private void Awake()
    {
        CreateLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void CreateLevel()
    {
        WorldMapPositionInitial = Vector3.zero;
        spawnPositions = new List<Vector2>();
        Tiles = new Dictionary<Point, Tile>();
        map = new GameObject("Map");
        string[] mapData = ReadLevelText();
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;
        MapPosition = new Vector2(mapX, mapY);
        Vector3 maxTile = Vector3.zero;
        Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        Debug.Log(worldStartPosition.x + " -x-y- "+ worldStartPosition.y);
        WorldMapPositionInitial= worldStartPosition;
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            Debug.Log(newTiles);
            for (int x = 0; x < mapX; x++)
            {
                //Debug.Log( "tile- " + newTiles[x] + " xvalue: " + x);
                //Debug.Log("tile- " + newTiles[x].ToString().Replace("\\", "OI") + " xvalue: " + x);
                if (newTiles[x] != '\n' && newTiles[x] != '\r' && newTiles[x].ToString() != "\r\n")
                {
                    PlaceTile(newTiles[x].ToString(), x, y, worldStartPosition);
                }                    
            }
        }
        Vector2 worldPositionNexus = SpawnNexus();
        //CreateOuterSquare(10);
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        SpawnDecorations(worldStartPosition, maxTile);

        RandomPointsGenerator = new RandomPointsGenerator(worldStartPosition, new Vector2(maxTile.x, maxTile.y), new Vector2Int(10, 10));
        MaxTile = new Vector3(maxTile.x + TileSize, maxTile.y - TileSize);
       
        //camera
        cameraMoviment.SetLimits(MaxTile);

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
        string dataString = data.text.Replace("\n", string.Empty);
        dataString = dataString.Replace("\r", string.Empty);
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
        NexusPoint = new Point(nexusPosX, nexusPosY);
        Tile tile = Tiles[NexusPoint];
        Vector2 worldPositionNexus = tile.PlaceNexus(nexusPrefab);
        return worldPositionNexus;

    }

    private void SpawnDecorations(Vector3 worldStartPosition, Vector3 worldEndPosition)
    {
        for(int i = 0;i<10;i++)
        {
            bool isCreated = false;
            do
            {
                int decorationPosX = UnityEngine.Random.Range(0, Math.Abs((int)worldEndPosition.x) + Math.Abs((int)worldStartPosition.x));
                int decorationPosY = UnityEngine.Random.Range(0, Math.Abs((int)worldEndPosition.y) + Math.Abs((int)worldStartPosition.y));
                Point DecPoint = new Point(decorationPosX, decorationPosY);
                Tile tile = Tiles[DecPoint];
                int randomDec = UnityEngine.Random.Range(0, 3);
                isCreated = tile.PlaceDecoration(decorations[randomDec]);
            } while (!isCreated);
            
        }
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

    private void CreateOuterSquare(int distance)
    {
        //distance = 100;
        int innerMapX = (int)MapPosition.x+distance*2;
        int innerMapY = (int)MapPosition.y+distance*2;
        int outerMapX = innerMapX + (2 * distance);
        int outerMapY = innerMapY + (2 * distance);
   
        Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // Calculate the starting position of the outer square
        Vector3 outerSquareStart = new Vector3(worldStartPosition.x - (TileSize * distance*2), worldStartPosition.y + (TileSize * distance*2), 0);

        // Create the outer square using the outerMapX and outerMapY values
        // You can use these values to determine the positions and sizes of the outer tiles or objects

        // Example code to create outer tiles
        for (int y = 0; y < outerMapY; y++)
        {
            for (int x = 0; x < outerMapX; y++)
            {
                Vector3 position = outerSquareStart + new Vector3(TileSize * x, -TileSize * y, 0);
                if (x < distance || x >= (innerMapX + distance) || y < distance || y >= (innerMapY + distance))
                {

                    // Create outer tile at position (x, y)
                    // Adjust the position based on the outerSquareStart and TileSize
                    Instantiate(tilePrefabs[3], position, Quaternion.identity);
                    spawnPositions.Add(position);
                }
                else if (x >= distance && x < (innerMapX + distance) && (y == distance || y == (innerMapY + distance - 1)))
                {
                    Instantiate(tilePrefabs[3], position, Quaternion.identity);
                    spawnPositions.Add(position);
                    // Create frame tile on the top and bottom rows of the inner square
                    // Adjust the position based on the outerSquareStart and TileSize
                    // Example: Instantiate(frameTilePrefab, outerSquareStart + new Vector3(TileSize * x, -TileSize * y, 0), Quaternion.identity);
                }
                else if ((x == distance || x == (innerMapX + distance - 1)) && y >= distance && y < (innerMapY + distance))
                {
                    Instantiate(tilePrefabs[3], position, Quaternion.identity);
                    spawnPositions.Add(position);
                    // Create frame tile on the left and right columns of the inner square
                    // Adjust the position based on the outerSquareStart and TileSize
                    // Example: Instantiate(frameTilePrefab, outerSquareStart + new Vector3(TileSize * x, -TileSize * y, 0), Quaternion.identity);
                }
            }
        }
    }
}
