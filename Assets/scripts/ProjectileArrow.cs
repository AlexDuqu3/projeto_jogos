using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{
    private Enemy enemy;
    private void Setup(Enemy enemy)
    {
        this.enemy = enemy; 
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir= (enemy.GetPosition() - transform.position).normalized;
        float moveSpeed = 10f;
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingOrder = 3;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
        transform.rotation = targetRotation;
        float destroySelfDistance = 1f;
        if(Vector3.Distance(transform.position, enemy.GetPosition()) < destroySelfDistance)
        {
            enemy.dealDamage(25);
            Destroy(gameObject);
        }
    }

    public void Shoot(Enemy enemy)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        Setup(enemy);
    }
}
