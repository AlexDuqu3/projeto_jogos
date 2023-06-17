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
        range =0.5f;
        shootTimerMax = 0.5f;
    

    }
    
    // Update is called once per frame
    void Update()
    {   shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;
            Vector3 closestEnemyPosition = GetClosestEnemy();
            if (closestEnemyPosition != Vector3.zero)
            {
                Shoot(closestEnemyPosition);
            }
        }
    }
    
    private void Shoot(Vector3 targetPosition)
    {
        Weapon weaponClass = weapon.GetComponent<Weapon>();
        if (weaponClass == null)
        {
            weaponClass = weapon.AddComponent<Weapon>();
        }
        weaponClass.Shoot(targetPosition);
    }

    private Vector3 GetClosestEnemy()
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
    }
  
    public float GetRange()
    {
        return range;
    }
}
