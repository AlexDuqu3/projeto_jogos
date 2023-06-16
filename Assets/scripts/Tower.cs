using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;

public class Tower : MonoBehaviour
{
    private Vector3 weaponPosition;
    [SerializeField]
    private GameObject arrow_1;
    private GameObject weapon_1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        weapon_1 = transform.Find("weapon").gameObject;
        weaponPosition = transform.Find("weapon").position;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //print to the console
            CMDebug.TextPopupMouse("Mouse Clicked");
            ProjectileArrow1.Create(arrow_1, weapon_1, weaponPosition, UtilsClass.GetMouseWorldPosition());
        }
    }
}
