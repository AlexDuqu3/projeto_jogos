using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    // Start is called before the first frame update
    private List<EnemySpawner> enemiesSpawners;
    [SerializeField]
    private GameObject enemySpawner;
    private GameObject spawner;

    private int enemiesAlive;

    public int EnemiesAlive { get => enemiesAlive; set => enemiesAlive = value; }

    void Start()
    {
        
    }

    public void AddEnemiesSpawners(int numberOfSpawn)
    {
        Vector2[] position = LevelManager.Instance.RandomPointsGenerator.GenerateRandomPoints(numberOfSpawn);

        foreach (Vector2 point in position)
        {
            EnemySpawner spawn = Instantiate(enemySpawner, point, Quaternion.identity).GetComponent<EnemySpawner>();
            spawn.transform.parent = spawner.transform;
            enemiesSpawners.Add(spawn);
        }
    }

    private void Awake()
    {
        enemiesSpawners = new List<EnemySpawner>();
        spawner = GameObject.Find("EnemySpawner");
        if (spawner == null)
        {
            spawner = new GameObject("EnemySpawner");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
