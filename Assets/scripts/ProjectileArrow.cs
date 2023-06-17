using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{
    private Vector3 targetPosition;
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
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingOrder = 3;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
        transform.rotation = targetRotation;
        float destroySelfDistance = 1f;
        if(Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
        {
            //reached the target
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 targetPosition)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        Setup(targetPosition);
    }
}
