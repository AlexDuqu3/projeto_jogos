using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusNonBuildArea : MonoBehaviour
{
    Point[] adjacentPoints;
    Tile tile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        //get the tile that the mouse is on
        tile = transform.parent.transform.parent.GetComponent<Tile>();
        adjacentPoints = tile.GetAdjacentPoints(5);
        //tile.ColorAdjacentTiles(new Color32(72, 72, 72, 255), adjacentPoints);
    }

    private void OnMouseExit()
    {
       
        //tile.ColorAdjacentTiles(Color.white, adjacentPoints);
    }
}
