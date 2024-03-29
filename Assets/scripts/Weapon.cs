using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject arrow;
    private Vector3 moveDir;
    public Animator animator;
    private bool isAnimtower1;
    private bool isAnimtower2;


    private void Awake()
    {
        arrow = transform.Find("arrow").gameObject;
        animator = gameObject.GetComponent<Animator>();
        isAnimtower1 = ContainsParam(animator, "onShoot");
        isAnimtower2 = ContainsParam(animator, "isShooting");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(Enemy enemy)
    {
        StartCoroutine(ShootCoroutine(enemy));
    }

    public void Aiming(Vector3 targetPosition)
    {
        moveDir = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
        float rotationSpeed = 50f;

        StartCoroutine(AimingCoroutine(targetRotation, rotationSpeed));

    }

    private IEnumerator ShootCoroutine(Enemy enemy)
    {
        
        Aiming(enemy.GetPosition());

        // Wait until the tower finishes aiming
        //yield return new WaitForSeconds(0.2f);
        while (IsAiming())
        {
            yield return null;
        }

        // Tower has finished aiming, proceed with shooting
        GameObject arrowObject = Instantiate(arrow, transform.position, Quaternion.identity);
        ProjectileArrow arrowClass = arrowObject.AddComponent<ProjectileArrow>();
        // animator.SetTrigger("Shoot");
        if (isAnimtower1)
            animator.SetTrigger("onShoot");
        if (isAnimtower2)
            animator.SetBool("isShooting", true);
        arrowClass.Shoot(enemy);
    }

    private IEnumerator AimingCoroutine(Quaternion targetRotation, float rotationSpeed)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private bool IsAiming()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, moveDir);
        float angleThreshold = 0.01f;

        // Compare the current rotation with the target rotation
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

        // If the angle difference is greater than the threshold, the tower is still aiming
        return angleDifference > angleThreshold;
    }

    public static bool ContainsParam(Animator _Anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }

    public void setAnim()
    {
        if (isAnimtower2)
            animator.SetBool("isShooting", false);
    }
}
