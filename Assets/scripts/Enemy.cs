using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static List<Enemy> enemyList = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        enemyList.Add(this);
    }

    public static Enemy Create(GameObject gObj,Vector3 position)
    {
        GameObject newEnemy = Instantiate(gObj,position, Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        return enemy;
    }
}
