using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManage : Singleton<GameManage>
{
    private GameObject towerPrefab;
    public Tower selectedTower { get; set; }
    public int Currency { get => currency; set => currency = value; }
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }
    private void Awake()
    {
        towerPrefab = Resources.Load<GameObject>("prefabs/tower_lvl1");
    }

    public int Lives;
    private int currency;

    private void Start()
    {
        Lives = 10;
        Currency = 50;
    }

    private void Update()
    {
        HandleEscape();
    }
    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();
    }
    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.level <= selectedTower.towerUpgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.price / 2;
            Tile selectedTile = selectedTower.GetComponentInParent<Tile>();
            selectedTile.IsEmpty = true;
            selectedTile.NumberOftowers--;
            selectedTile.MarkAdjacentPointsPristine();
            selectedTower.Sell();
            DeselectTower();
        }
    }

    private void HandleEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(selectedTower != null)
            {
                DeselectTower();
            }
            else
            {
                Application.Quit();
            }
        }   
    }


}
