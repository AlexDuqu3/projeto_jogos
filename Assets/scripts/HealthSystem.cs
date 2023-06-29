using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{

    public event EventHandler onHealthChanged;

    private int healthMax;
    private int health;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        this.health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetHealthMax()
    {
        return healthMax;
    }

    public int Damage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }
        this.onHealthChanged?.Invoke(this, EventArgs.Empty);
        //OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die();
        }
        return health;
    }

    public void Die()
    {
        //OnDead?.Invoke(this, EventArgs.Empty); -> animacao de quando morre i guess??
        
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public float GetHealthPercent()
    {
        return (float)health / healthMax;
    }
}
