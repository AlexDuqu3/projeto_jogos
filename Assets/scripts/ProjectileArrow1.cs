using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileArrow1 : MonoBehaviour
{
    private Vector3 targetPosition;
    private GameObject arrow_1;
    private GameObject weapon_1;
    public static void Create(GameObject arrow_1_object,GameObject weapon_1, Vector3 spawnPosition, Vector3 targetPosition)
    {
        //arrow
       GameObject arrowTransform= Instantiate(arrow_1_object, spawnPosition, Quaternion.identity);
        ProjectileArrow1 projectileArrow1 = arrowTransform.AddComponent<ProjectileArrow1>(); // Add the ProjectileArrow1 component to the instantiated arrow
        projectileArrow1.arrow_1 = arrow_1_object;
        projectileArrow1.weapon_1 = weapon_1;
        projectileArrow1.Setup(targetPosition);

    }

    private void Setup(Vector3 targetPosition)
    {
        this.targetPosition= targetPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir= (targetPosition - transform.position).normalized;
        float moveSpeed = 10f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
        transform.rotation = targetRotation;
        float rotationSpeed = 10f;
        weapon_1.transform.rotation = Quaternion.Slerp(weapon_1.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); ;
        float destroySelfDistance = 1f;
        if(Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
        {
            //reached the target
            Destroy(gameObject);
        }
    }
}
