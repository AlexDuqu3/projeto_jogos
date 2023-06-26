
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int[] percentages;
    [SerializeField]
    private int maxEnemies;
    private GameObject enemy;

    //mais variaveis para cada tipo de inimigo
    private void Awake()
    {
        enemy = GameObject.Find("Enemy");
        if (enemy == null)
        {
            enemy = new GameObject("Enemy");
        }
    }
    [SerializeField]
    private float interval = 1.5f;

    private List<Item> mobsPercentage;
    void Start()
    {
        mobsPercentage = new List<Item>();
        for (int i = 0; i < percentages.Length; i++)
        {
            mobsPercentage.Add(new Item() { enemy = enemies[i],chance = percentages[i] });
        }

        StartCoroutine(spawnEnemy(interval,maxEnemies, mobsPercentage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, int maxEnemies, List<Item> mobsPercentage)
    {
        yield return new WaitForSeconds(interval);
        Item enemyPercent = ProportionalWheelSelection.SelectItem(mobsPercentage);
        Enemy enemyObj = Enemy.Create(enemyPercent.enemy, new Vector3(Random.Range(-5f, 5)+transform.position.x, Random.Range(-6f, 6f)+transform.position.y, 0));
        enemyObj.transform.parent = enemy.transform;
        maxEnemies -= 1;
        if(maxEnemies > 0)
        {
            StartCoroutine(spawnEnemy(interval, maxEnemies, mobsPercentage));
        }
        
    }

    /*private IEnumerator spawnEnemy(float interval, List<Item> mobsPercentage)
    {
        yield return new WaitForSeconds(interval);
        Item enemyPercent = ProportionalWheelSelection.SelectItem(mobsPercentage);
        Enemy enemyObj = Enemy.Create(enemyPercent.enemy, new Vector3(Random.Range(-5f, 5) + transform.position.x, Random.Range(-6f, 6f) + transform.position.y, 0));
        enemyObj.transform.parent = enemy.transform;
        StartCoroutine(spawnEnemy(interval, mobsPercentage));
    }*/

    public void Spawn(Vector2 position)
    {
        
    }

}
