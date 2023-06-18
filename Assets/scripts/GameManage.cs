using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManage : Singleton<GameManage>
{
    public Tower selectedTower { get; set; }
    public int Lives;
    public int Currency;

    private void Start()
    {
        Lives = 10;
        Currency = 50;
    }
    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();
    }
    public void DeselectTower()
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.level<=selectedTower.towerUpgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

}
