﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : Singleton<GameManage>
{
    public TowerPlacement TowerPlacementBtn { get; private set; }
    public Tower selectedTower { get; set; }
    private Text currencyText;
    private Text healthText;
    private Text waveText;
    [SerializeField]
    private GameObject EnemySpawner;
    private bool gameOver = false;
    public ObjectPool Pool { get; set; }
    private int towersAdjacentRadius;
    public int TowersAdjacentRadius
    {
        get
        {
            return towersAdjacentRadius;
        }
        set
        {
            towersAdjacentRadius = value;
        }
    }
    private int currency;
    public int Currency
    {
        get { return currency; }
        set
        {
            currency = value;
            currencyText.text = value.ToString() + "<color=lime>$</color>";
        }
    }
    private int health;
    public int Health
    {
        get => health; set
        {
            health = value;
            if (health <= 0)
            {
                health = 0;
                GameOver();
            }
            healthText.text = health.ToString() + "<color=red>♥</color>";

        }
    }
    private int wave;
    public int Wave
    {
        get => wave; set
        {
            wave = value;
            //waveText.text = "Wave/"+wave.ToString();
        }
    }
    private void Awake()
    {
       // waveText= GameObject.Find("Stats/WaveStats").GetComponent<Text>();
        currencyText = GameObject.Find("CurrencyText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        Pool = GetComponent<ObjectPool>();
    }


    private void Start()
    {
        Health = 10;
        Currency = 50000;
        TowersAdjacentRadius = 3; //3x3
        Wave = 1;
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
            selectedTile.IsEmpty = true; // clear the tile
            selectedTile.NumberOftowers--; // decrement the number of towers "using" the tile (adjacentS)
            selectedTile.MarkAdjacentPointsPristine(selectedTile.GetAdjacentPoints(GameManage.Instance.TowersAdjacentRadius));//3x3
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
        if (Currency >= towerPlacement.Price)
        {
            TowerPlacementBtn = towerPlacement;
            Hover.Instance.Activate(towerPlacement.TowerPrefab.GetComponent<SpriteRenderer>().sprite);
        }

    }

    public void BuyTower()
    {
        if (Currency >= TowerPlacementBtn.Price)
        {
            Currency -= TowerPlacementBtn.Price;
            TowerPlacementBtn = null;
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
        }
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        //int monsterIndex = Random.Range(0, 4);
        //string type = string.Empty;
        //switch (monsterIndex)
        //{
        //    case 0:
        //        type = "BlueMonster";
        //        break;
        //    case 1:
        //        type = "GreenMonster";
        //        break;
        //    case 2:
        //        type = "PurpleMonster";
        //        break;
        //    case 3:
        //        type = "RedMonster";
        //        break;
        //}   
        //Pool.GetObject(type).GetComponent<Enemy>();
        Vector2 spawnPoint = LevelManager.Instance.GetRandomPointOutsideMap(10);
        bool foundValidSpawnPoint = false;

        while (!foundValidSpawnPoint)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint, 1f);

            if (colliders.Length == 0)
            {
                // No colliders found, it's a valid spawn point
                foundValidSpawnPoint = true;
                Instantiate(EnemySpawner, spawnPoint, Quaternion.identity);
            }
            else
            {
                // There are colliders, try finding another point
                spawnPoint = LevelManager.Instance.GetRandomPointOutsideMap(10);
            }
        }

        //criar spawners (game ObjecT) at the points
        // Instance.Pool.GetObject("Spawner").GetComponent<EnemySpawner>().Spawn(point);
        yield return new WaitForSeconds(2.5f);
    }

}
