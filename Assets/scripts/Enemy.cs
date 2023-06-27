using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    public static List<Enemy> enemyList = new List<Enemy>();
    private HealthSystem healthSystem;
    private float obstacleDetectionDistance = 1.5f;
    private float obstacleAvoidanceStrength = 0.5f*3;
    private LayerMask obstacleLayerMask;
    private HealthBar healthBar;
    private Transform pfHealthBar;
    private EnemySpawner parentSpawner;

    public EnemySpawner ParentSpawner { get => parentSpawner; set => parentSpawner = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveTowardNexus();
    }

    private void Awake()
    {
        enemyList.Add(this);
        this.healthSystem = new HealthSystem(110);
        GameObject HealthBarGO = transform.Find("HealthBarGO").gameObject;
        this.healthBar = HealthBarGO.GetComponent<HealthBar>();
        this.healthBar.Setup(this.healthSystem);
        obstacleLayerMask = LayerMask.GetMask("Tower");
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
                this.healthBar.destroyThis();
                parentSpawner.RemoveEnemy(this);
                Destroy(gameObject);
                //Destroy(this);
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

    public void moveTowardNexus()
    {
        Vector3 nexusWorldPosition = LevelManager.Instance.Tiles[LevelManager.Instance.NexusPoint].WorldPosition;
        Vector3 direction = (nexusWorldPosition - GetPosition()).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayerMask);
        if (hit.collider != null && hit.collider.CompareTag("Tower"))
        {
            // A torre foi detectada, calcular um novo vetor de direção para desviar da torre
            Vector3 obstacleAvoidanceDirection = CalculateObstacleAvoidanceDirection(hit.normal);

            // Aplicar o novo vetor de direção com o desvio da torre
            direction = (direction + obstacleAvoidanceDirection).normalized;
        }
        transform.position += direction * Time.deltaTime * 2f;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, -direction);
        transform.rotation = targetRotation;
    }

    private Vector3 CalculateObstacleAvoidanceDirection(Vector3 obstacleNormal)
    {
        // Calcular um vetor de desvio lateral baseado na normal da torre
        Vector3 avoidanceDirection = Vector3.Cross(Vector3.forward, obstacleNormal);
        return avoidanceDirection.normalized * obstacleAvoidanceStrength;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Nexus"))
        {
            Nexus nexus = collision.GetComponent<Nexus>();
            if (nexus != null)
            {
                nexus.LoseHealth(1);
                StartCoroutine(Scale(transform.localScale, new Vector3(0.1f,0.1f),true));
                //Destroy(gameObject);
            }
        }
    }
    public IEnumerator Scale(Vector3 from,Vector3 to,bool remove)
    {
        float progress= 0;
        while (progress <= 1){
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
        if (remove) {
            parentSpawner.RemoveEnemy(this);
            Destroy(gameObject);
                };
    }
}
