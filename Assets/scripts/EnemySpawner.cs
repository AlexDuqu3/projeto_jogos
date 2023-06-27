
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemiesPrefab;
    [SerializeField]
    private int[] percentages;
    [SerializeField]
    private int maxEnemies;
    private GameObject enemy;
    private List<Enemy> enemies;
    //mais variaveis para cada tipo de inimigo
    [SerializeField]
    private float interval = 1.5f;
    private bool isSpawning;
    private void Awake()
    {
        enemies= new List<Enemy>();
        enemy = GameObject.Find("Enemy");
        if (enemy == null)
        {
            enemy = new GameObject("Enemy");
        }
        mobsPercentage = new List<Item>();
        for (int i = 0; i < percentages.Length; i++)
        {
            mobsPercentage.Add(new Item() { enemy = enemiesPrefab[i], chance = percentages[i] });
        }
    }
    private List<Item> mobsPercentage;

    public bool IsSpawning { get => isSpawning; private set => isSpawning = value; }
    public List<Enemy> Enemies { get => enemies; private set => enemies = value; }

    void Start()
    {
       

        //spawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async public void spawnEnemy()
    {
        IsSpawning=true;
        Item enemyPercent = ProportionalWheelSelection.SelectItem(mobsPercentage);
        for (int i = 0; i < maxEnemies; i++)
        {
        Enemy enemyObj = Enemy.Create(enemyPercent.enemy, new Vector3(Random.Range(-5f, 5)+transform.position.x, Random.Range(-6f, 6f)+transform.position.y, 0));
            enemyObj.ParentSpawner=this;
        enemyObj.transform.parent = enemy.transform;
            Enemies.Add(enemyObj);
            await Task.Delay(3000);
        }   
        IsSpawning=false;
    }

    /*private IEnumerator spawnEnemy(float interval, List<Item> mobsPercentage)
    {
        yield return new WaitForSeconds(interval);
        Item enemyPercent = ProportionalWheelSelection.SelectItem(mobsPercentage);
        Enemy enemyObj = Enemy.Create(enemyPercent.enemy, new Vector3(Random.Range(-5f, 5) + transform.position.x, Random.Range(-6f, 6f) + transform.position.y, 0));
        enemyObj.transform.parent = enemy.transform;
        StartCoroutine(spawnEnemy(interval, mobsPercentage));
    }*/
    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }
    public void Spawn(Vector2 position)
    {
        
    }

}
