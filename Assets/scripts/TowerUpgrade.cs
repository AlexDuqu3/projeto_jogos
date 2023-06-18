using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{
    public int Price { get; private set; }
    public int Damage { get; private set; }
    public int Range { get; private set; }
    public float DebuffDuration { get; private set; }
    public float ProcChance { get; private set; }
    public float SlowingFactor { get; private set; }
    public float TickTime { get; private set; }
    public int SpecialDamage { get; private set; }
    public GameObject towerVisual { get; private set; }

    public TowerUpgrade(int price, int damage, int range, GameObject towerVisual)
    {
        this.Price = price;
        this.Damage = damage;
        this.Range = range;
        this.towerVisual = towerVisual;
    }
    public TowerUpgrade(int price, int damage, int range, float debuffDuration, float procChance)
    {
        this.Price = price;
        this.Damage = damage;
        this.Range = range;
        this.DebuffDuration = debuffDuration;
        this.ProcChance = procChance;
    }
    public TowerUpgrade(int price, int damage, int range, float debuffDuration, float procChance, float slowingFactor, float tickTime, int specialDamage)
    {
        this.Price = price;
        this.Damage = damage;
        this.Range = range;
        this.DebuffDuration = debuffDuration;
        this.ProcChance = procChance;
        this.SlowingFactor = slowingFactor;
        this.TickTime = tickTime;
        this.SpecialDamage = specialDamage;
    }   
}
