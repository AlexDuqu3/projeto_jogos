using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    private Tile tile;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            //get the tile that the mouse is on 
            tile = LevelManager.Instance.GetTileAtWorldPosition();
            if(tile != null)
            {
                spriteRenderer.sortingOrder =tile.GridPosition.Y;
            }
        }

        if(!spriteRenderer.enabled && GameManage.Instance.TowerPlacementBtn != null)
        {
            tile.PlaceTower();
            Deactivate();
        }
    }
    public void Activate(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
    }
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
    }
}
