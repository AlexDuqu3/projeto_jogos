using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class Tower : MonoBehaviour
{
    private GameObject weapon;
    private GameObject colliderObject;
    [Range(0f, 20f)]
    public float range;
    [SerializeField]
    private float shootTimerMax;
    private float shootTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        weapon = transform.Find("weapon").gameObject;
        colliderObject = transform.Find("range").gameObject;
        shootTimerMax = 1f;
        //range = 10f
    }
    
    // Update is called once per frame
    void Update()
    {   shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;
            /*Vector3 closestEnemyPosition = GetClosestEnemy();
            if (closestEnemyPosition != Vector3.zero)
            {
                Shoot(closestEnemyPosition);
            }*/
            Enemy enemy = GetClosestEnemy();
            if (enemy != null)
            {
                Shoot(enemy);
            }
        }
    }
    
    private void Shoot(Enemy enemy)
    {
        Weapon weaponClass = weapon.GetComponent<Weapon>();
        if (weaponClass == null)
        {
            weaponClass = weapon.AddComponent<Weapon>();
        }
        weaponClass.Shoot(enemy);
    }

   /* private Vector3 GetClosestEnemy()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        UpgradeOverlay colliderClass = colliderObject.GetComponent<UpgradeOverlay>();
        if (colliderClass == null)
        {
            colliderClass = colliderObject.AddComponent<UpgradeOverlay>();
        }
        if (colliderClass.IsColliding())
        {
            // Mouse position is within the circular range
            return mousePosition;
        }

        return Vector3.zero;
    }*/
  
    public float GetRange()
    {
        return range;
    }

    private Enemy GetClosestEnemy()
    {
        Enemy enemy = Enemy.GetClosestEnemy(transform.position, GetRange());
        return enemy;
    }
}
