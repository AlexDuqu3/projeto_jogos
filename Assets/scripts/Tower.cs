using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Drawing;

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

    public Button panelUpgradeButton, panelSellButton;

    [SerializeField] private AudioSource upgradeSound, sellSound, shotSound;

    private GameObject rangeCircle;


    public int level;
    private int price;
    public int Price
    {
        get
        {
            return price;
        }
        set { price = value; }
    }
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
        Debug.Log("Void Tower Awake");
        level= 1;
        BindingUpgradePanelButtons();
        weapon = transform.Find("weapon").gameObject;
        colliderObject = transform.Find("range").gameObject;
        upgradePanel = transform.Find("upgradePanel").gameObject;
        rangeCircle = transform.Find("RangeCircle").gameObject;

        Debug.Log(upgradePanel.gameObject.ToString());


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
            else
            {
                Weapon weaponClass = weapon.GetComponent<Weapon>();
                if (weaponClass != null)
                {
                    weaponClass.setAnim();
                }
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
        Debug.Log("Shoot");
        Debug.Log(shotSound);
        SoundManager.Instance.PlayAudioSource(shotSound);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision");
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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManage.Instance.selectedTower != null)
                {
                    GameManage.Instance.DeselectTower(this);
                    return;
                }
                GameManage.Instance.SelectTower(this);
            }
        }
    }

    public virtual string GetStats()
    {
        return string.Format("\nLevel: { 0}\nDamage: { 1}\nRange: { 2}", level, damage, range);
    }

    public void Select()
    {
        //Selects a tower a displays the upgrade panel TODO: maybe we should display also the towers stats and the tower range object here.
        rangeCircle.SetActive(true);
        upgradePanel.SetActive(!upgradePanel.activeSelf);
        drawCircle();
    }

    public void Deselect()
    {
        rangeCircle.SetActive(false);
    }

    private void drawCircle()
    {
        LineRenderer LineDrawer = rangeCircle.GetComponent<LineRenderer>();
        float Theta = 0f;
        float ThetaScale = 0.01f;
        int Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;
        LineDrawer.startWidth = 0.11f;
        LineDrawer.endWidth = 0.11f;

        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = range * Mathf.Cos(Theta);
            float y = range * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(x + transform.position.x, y + transform.position.y));
        }
    }

    public virtual void Upgrade()
    {
        Debug.Log("Dentro do Upgrade");
        SoundManager.Instance.PlayUpgradeSound();
        GameManage.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.range += NextUpgrade.Range;
        GameObject newTowerObject=Instantiate(NextUpgrade.towerVisual, transform.position, transform.rotation, transform.parent);
        this.level += 1;
        Tower newTowerClass= newTowerObject.GetComponent<Tower>();
        newTowerClass.damage = damage;
        newTowerClass.range = range;
        newTowerClass.Price = Price;
        newTowerClass.level = level;
        newTowerClass.towerUpgrades = towerUpgrades;
        newTowerClass.GetComponent<Tower>().damage = damage;
        newTowerClass.GetComponent<Tower>().range = range;
        newTowerClass.GetComponent<SpriteRenderer>().sortingOrder=gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        GameManage.Instance.SelectTower(newTowerClass);

        Destroy(gameObject);
        if (newTowerObject.transform.Find("upgradePanel").gameObject.transform.Find("upgrade") != null)
        {
            GameObject upgradeButton = newTowerObject.transform.Find("upgradePanel").gameObject.transform.Find("upgrade").gameObject;
            upgradeButton.GetComponent<Button>().onClick.AddListener(() => GameManage.Instance.UpgradeTower());
        }
        GameObject sellButton= newTowerObject.transform.Find("upgradePanel").gameObject.transform.Find("sell").gameObject;
        sellButton.GetComponent<Button>().onClick.AddListener(() => GameManage.Instance.SellTower());
    }

    public void Sell()
    {
        SoundManager.Instance.PlaySellSound();

        Destroy(gameObject);
    }

    protected void BindingUpgradePanelButtons()
    {
        Debug.Log("aqui no biding");
        if(transform.Find("upgradePanel").gameObject.transform.Find("upgrade") != null)
        {
            GameObject upgradeButton = transform.Find("upgradePanel").gameObject.transform.Find("upgrade").gameObject;
            Debug.Log("AQUIIII");
            //Debug.Log(upgradeButton);
            Button upgradeButtonComponent = upgradeButton.GetComponent<Button>();
            //Button upgradeButtonComponent = upgradeButton;
            Debug.Log(upgradeButtonComponent.interactable.ToString());

            if (!(upgradeButtonComponent.onClick.GetPersistentEventCount() > 0))
            {
                upgradeButtonComponent.onClick.AddListener(() => GameManage.Instance.UpgradeTower());
            }
        }
        
        GameObject sellButton = transform.Find("upgradePanel").gameObject.transform.Find("sell").gameObject;
        Button sellButtonComponent = sellButton.GetComponent<Button>();
        if (!(sellButtonComponent.onClick.GetPersistentEventCount() > 0))
        {
            sellButtonComponent.onClick.AddListener(() => GameManage.Instance.SellTower());
        }
        
    }
    private Enemy GetClosestEnemy()
    {
        Enemy enemy = Enemy.GetClosestEnemy(transform.position, GetRange());
        return enemy;
    }
}
