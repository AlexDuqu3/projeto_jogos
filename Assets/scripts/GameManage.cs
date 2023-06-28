using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    private Text timerText;
    public GameOverScreen gameOverScreen;
    [SerializeField]
    private GameObject EnemySpawner;
    private bool gameOver = false;
    public ObjectPool Pool { get; set; }
    private int towersAdjacentRadius;
    private float waveDelay;
    private bool waitingForNextWave;
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
            waveText.text = "Wave/"+wave.ToString();
        }
    }
    private float waveTimer;
    public float WaveTimer
    {
        get => waveTimer; set
        {
            waveTimer = value;
            // Convert elapsed time to TimeSpan
            TimeSpan timeSpan = TimeSpan.FromSeconds(waveTimer);

            // Format the time string
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timerText.text = formattedTime.ToString();
        }
    }
    private void Awake()
    {
        timerText = GameObject.Find("Stats/TimerText").GetComponent<Text>();
        waveText = GameObject.Find("Stats/WaveText").GetComponent<Text>();
        currencyText = GameObject.Find("CurrencyText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        Pool = GetComponent<ObjectPool>();
    }


    private void Start()
    {
        Health = 10;
        Currency = 400;
        TowersAdjacentRadius = 3; //3x3
        Wave = 0;
        waveDelay = 10f;
        waitingForNextWave = false;
    }

    private void Update()
    {
        HandleEscape();
        if (!IsWaveActive() && !waitingForNextWave)
        {
            // Iniciar a contagem regressiva para a próxima wave
            waitingForNextWave = true;
            waveTimer = waveDelay;
        }

        if (waitingForNextWave)
        {
            WaveTimer = waveTimer;
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0f)
            {
                StartWave();
                waveTimer = waveDelay;
                waitingForNextWave = false;
            }
        }
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
    public void DeselectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower.Deselect();
        selectedTower = null;
        if (tower != null)
        {
            selectedTower = tower;
            selectedTower.Select();
        }
    }

    public void UpgradeTower()
    {
        Debug.Log("UpgradeTowerFunciotn-GameManager");
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
            DeselectTower(selectedTower);
            selectedTower.Sell();
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedTower != null)
            {
                DeselectTower(selectedTower);
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

    public void DropTower(TowerPlacement towerPlacement) {
        
        TowerPlacementBtn = towerPlacement;
        Hover.Instance.Deactivate();
        
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
            gameOverScreen.Setup();
        } 
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        WaveTimer = 0f;
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

        //List<Vector2> position = LevelManager.Instance.SpawnPositions;
        Wave += 1;
        EnemyManager.Instance.AddEnemiesSpawners(Wave+2);
        EnemyManager.Instance.spawnAll();

        yield return new WaitForSeconds(2.5f);
    }

    private bool IsWaveActive()
    {
        return !EnemyManager.Instance.IsAllSpawnersDone() || !EnemyManager.Instance.IsAllEnemiesDead();
    }
}
