using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    private GameObject weapon;
    private GameObject colliderObject;
    protected int damage;
    [Range(0f, 20f)]
    public float range;
    [SerializeField]
    protected float shootTimerMax;
    private float shootTimer;
    public TowerUpgrade[] towerUpgrades { get; protected set; }
    private GameObject upgradePanel;
    public int level;
    public int price;
    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (towerUpgrades.Length > level - 1)
            {
                return towerUpgrades[level - 1];
            }
            return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public virtual void Awake()
    {
        level= 1;
        BindingUpgradePanelButtons();
        weapon = transform.Find("weapon").gameObject;
        colliderObject = transform.Find("range").gameObject;
        upgradePanel = transform.Find("upgradePanel").gameObject;

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

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManage.Instance.selectedTower != null)
            {
                GameManage.Instance.DeselectTower();
                return;
            }
            GameManage.Instance.SelectTower(this);
        }
    }

    public virtual string GetStats()
    {
        return string.Format("\nLevel: { 0}\nDamage: { 1}\nRange: { 2}", level, damage, range);
    }

    public void Select()
    {
        //Selects a tower a displays the upgrade panel TODO: maybe we should display also the towers stats and the tower range object here.
        upgradePanel.SetActive(!upgradePanel.activeSelf);
    }

    public virtual void Upgrade()
    {
        GameManage.Instance.Currency -= NextUpgrade.Price;
        price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.range += NextUpgrade.Range;
        GameObject newTowerObject=Instantiate(NextUpgrade.towerVisual, transform.position, transform.rotation, transform.parent);
        this.level += 1;
        Tower newTowerClass= newTowerObject.GetComponent<Tower>();
        newTowerClass.damage = damage;
        newTowerClass.range = range;
        newTowerClass.price = price;
        newTowerClass.level = level;
        newTowerClass.towerUpgrades = towerUpgrades;
        newTowerClass.GetComponent<Tower>().damage = damage;
        newTowerClass.GetComponent<Tower>().range = range;
        GameManage.Instance.SelectTower(newTowerClass);
        Destroy(gameObject);
        GameObject upgradeButton=newTowerObject.transform.Find("upgradePanel").gameObject.transform.Find("upgrade").gameObject;
        GameObject sellButton= newTowerObject.transform.Find("upgradePanel").gameObject.transform.Find("sell").gameObject;
        sellButton.GetComponent<Button>().onClick.AddListener(() => GameManage.Instance.SellTower());
        upgradeButton.GetComponent<Button>().onClick.AddListener(() => GameManage.Instance.UpgradeTower());
    }

    public void Sell()
    {
        Destroy(gameObject);
    }

    protected void BindingUpgradePanelButtons()
    {
        GameObject upgradeButton = transform.Find("upgradePanel").gameObject.transform.Find("upgrade").gameObject;
        GameObject sellButton = transform.Find("upgradePanel").gameObject.transform.Find("sell").gameObject;
        Button sellButtonComponent = sellButton.GetComponent<Button>();
        if (!(sellButtonComponent.onClick.GetPersistentEventCount() > 0))
        {
            sellButtonComponent.onClick.AddListener(() => GameManage.Instance.SellTower());
        }
        Button upgradeButtonComponent = upgradeButton.GetComponent<Button>();
        if (!(upgradeButtonComponent.onClick.GetPersistentEventCount() > 0))
        {
            upgradeButtonComponent.onClick.AddListener(() => GameManage.Instance.UpgradeTower());
        }
    }
    private Enemy GetClosestEnemy()
    {
        Enemy enemy = Enemy.GetClosestEnemy(transform.position, GetRange());
        return enemy;
    }
}
