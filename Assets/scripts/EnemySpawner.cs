
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

    //mais variaveis para cada tipo de inimigo

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

        StartCoroutine(spawnEnemy(interval, mobsPercentage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, List<Item> mobsPercentage)
    {
        yield return new WaitForSeconds(interval);
        Item enemyPercent = ProportionalWheelSelection.SelectItem(mobsPercentage);
        Enemy enemyObj = Enemy.Create(enemyPercent.enemy, new Vector3(Random.Range(-5f, 5), Random.Range(-6f, 6f), 0));
        StartCoroutine(spawnEnemy(interval, mobsPercentage));
    }

}
