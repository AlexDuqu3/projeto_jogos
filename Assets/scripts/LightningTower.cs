using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTower : Tower
{
    // Start is called before the first frame update
    void Start()
    {
        towerUpgrades = new TowerUpgrade[]
      {
            new TowerUpgrade(100,75,8,Resources.Load<GameObject>("prefabs/tower2_lvl2")),
            new TowerUpgrade(150,130,11,Resources.Load<GameObject>("prefabs/tower2_lvl3")),
      };
    }

    public override void Awake()
    {
        range = 4f;
        shootTimerMax = 0.5f;
        damage = 45;
        base.Awake();

    }
}
