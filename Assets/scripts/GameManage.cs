using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : Singleton<GameManage>
{
    private GameObject towerPrefab;
    public TowerPlacement TowerPlacementBtn { get; private set; }
    public Tower selectedTower { get; set; }
    private Text currencyText;
    private int currency;

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    public int Currency
    {
        get { return currency; }
        set
        {
            currency = value;
            currencyText.text = value.ToString() + "<color=lime>$</color>";
        }
    }

    private void Awake()
    {
        currencyText = GameObject.Find("CurrencyText").GetComponent<Text>();
        towerPrefab = Resources.Load<GameObject>("prefabs/tower_lvl1");
    }
    public int Lives;

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
            Currency += selectedTower.Price / 2;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedTower != null)
            {
                DeselectTower();
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void PickTower(TowerPlacement towerPlacement)
    {
        TowerPlacementBtn = towerPlacement;
        Hover.Instance.Activate(towerPlacement.TowerPrefab.GetComponent<SpriteRenderer>().sprite);
    }

    public void BuyTower()
    {
        TowerPlacementBtn = null;
    }

}
