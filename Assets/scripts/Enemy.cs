using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    public static List<Enemy> enemyList = new List<Enemy>();
    private HealthSystem healthSystem;
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
        this.healthSystem = new HealthSystem(100);
        int a = 1;
    }

    public void dealDamage(int dmg)
    {
        if (healthSystem != null)
        {
            int health = healthSystem.Damage(dmg);
            if (health <= 0)
            {
                //
                Enemy.enemyList.Remove(this);
                Destroy(gameObject);
                Destroy(this);
            }
        }
    }

    public static Enemy GetClosestEnemy(Vector3 position, float maxRange)
    {
        Enemy closest = null;
        foreach (Enemy enemy in enemyList)
        {
            if(enemy == null) continue;
            if (enemy.IsDead()) continue;
            float g = Vector3.Distance(position, enemy.GetPosition());
            if (g <= maxRange)
            {
                if (closest == null)
                {
                    closest = enemy;
                }
                else
                {
                    if (Vector3.Distance(position, enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition()))
                    {
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }

    public static Enemy Create(GameObject gObj,Vector3 position)
    {
        GameObject newEnemy = Instantiate(gObj,position, Quaternion.identity);
        Enemy enemy = newEnemy.GetComponent<Enemy>();
        //Enemy ene =  Enemy(newEnemy);
        //enemy.healthSystem = new HealthSystem(100);
        //enemyList.Add(enemy);
        return enemy;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
}
