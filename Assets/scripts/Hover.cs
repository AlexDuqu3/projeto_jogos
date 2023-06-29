using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    private Tile tile;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; private set => spriteRenderer = value; }

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        if (SpriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //get the tile that the mouse is on 
            tile = LevelManager.Instance.GetTileAtWorldPosition();
            if(tile != null)
            {
                SpriteRenderer.sortingOrder =tile.GridPosition.Y;
            }
        }

        if(!SpriteRenderer.enabled && GameManage.Instance.TowerPlacementBtn != null)
        {
            if(tile != null)
            {
            tile.PlaceTower();
            }
            Deactivate();

        }
    }
    public void Activate(Sprite sprite)
    {
        SpriteRenderer.sprite = sprite;
        SpriteRenderer.enabled = true;
    }
    public void Deactivate()
    {
        SpriteRenderer.enabled = false;
    }
}
