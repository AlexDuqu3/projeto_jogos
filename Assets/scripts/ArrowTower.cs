using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTower : Tower
{
    // Start is called before the first frame update

    //[SerializeField] private Button _upgrade, _sell;
    void Start()
    {
        towerUpgrades = new TowerUpgrade[]
       {
            new TowerUpgrade(50,50,9,Resources.Load<GameObject>("prefabs/tower_lvl2")),
            new TowerUpgrade(50,75,12,Resources.Load<GameObject>("prefabs/tower_lvl3")),
       };
    }


    public override void Awake()
    {
        range = 4f;
        shootTimerMax = 0.5f;
        damage = 25;
        //panelUpgradeButton = _upgrade;
        //panelSellButton = _sell;
        base.Awake();
    }

}
