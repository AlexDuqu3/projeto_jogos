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
    public Dictionary<Point, Tile> Tiles { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, Tile>();
        map = new GameObject("Map");
        string[] mapData = ReadLevelText();
        int mapX = mapData[0].ToCharArray().Length;
        int mapY= mapData.Length;
        Vector3 maxTile = Vector3.zero;
        Vector3 worldStartPosition=Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height));
        for(int y=0;y< mapY; y++)
        {
            char[] newTiles= mapData[y].ToCharArray();
            for(int x=0;x< mapX; x++)
            {
                Tile newTile=PlaceTile(newTiles[x].ToString(),x,y, worldStartPosition);
                newTile.IsDisable = IsBorderTile(x, y, mapX, mapY);
            }
        }
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMoviment.SetLimits(new Vector3(maxTile.x+TileSize,maxTile.y-TileSize));
    }

    private Tile PlaceTile(string tileType,int x,int y,Vector3 worldStartPosition)
    {
        int tileIndex=int.Parse(tileType);
        Tile newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<Tile>();
        newTile.Setup(new Point(x, y), new Vector3(worldStartPosition.x + (TileSize * x), worldStartPosition.y - (TileSize * y), 0),map.transform);
        return newTile;
    }

    private bool IsBorderTile(int x, int y,int mapX,int mapY)
    {
        // Check if the tile is on the border
        return x == 0 || y == 0 || x == mapX - 1 || y == mapY - 1;
    }

    private string[] ReadLevelText()
    {
        TextAsset data=Resources.Load("Level") as TextAsset;
        string dataString = data.text.Replace(Environment.NewLine, string.Empty);
        return dataString.Split('-');
    }
}
