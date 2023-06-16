using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject beetlePrefab;
    //mais variaveis para cada tipo de inimigo

    [SerializeField]
    private float interval = 10.5f;
    void Start()
    {
        StartCoroutine(spawnEnemy(interval, beetlePrefab));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);  
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f,5), Random.Range(-6f, 6f),0),Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

}
