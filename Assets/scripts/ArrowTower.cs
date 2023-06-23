using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : Tower
{
    // Start is called before the first frame update
    void Start()
    {
        towerUpgrades = new TowerUpgrade[]
       {
            new TowerUpgrade(2,2,2,Resources.Load<GameObject>("prefabs/tower_lvl2")),
            new TowerUpgrade(3,3,3,Resources.Load<GameObject>("prefabs/tower_lvl3")),
       };
    }


    public override void Awake()
    {
        range = 0.5f;
        shootTimerMax = 0.5f;
        damage = 1;
        base.Awake();
    }

}
