using System;
using System.Collections.Generic;
using UnityEngine;

public class ProportionalWheelSelection
{
    public static System.Random rnd = new System.Random();

    // Static method for using from anywhere. You can make its overload for accepting not only List, but arrays also: 
    // public static Item SelectItem (Item[] items)...
    public static Item SelectItem(List<Item> items)
    {
        // Calculate the summa of all portions.
        int poolSize = 0;
        for (int i = 0; i < items.Count; i++)
        {
            poolSize += items[i].chance;
        }

        // Get a random integer from 0 to PoolSize.
        int randomNumber = rnd.Next(0, poolSize) + 1;

        // Detect the item, which corresponds to current random number.
        int accumulatedProbability = 0;
        for (int i = 0; i < items.Count; i++)
        {
            accumulatedProbability += items[i].chance;
            if (randomNumber <= accumulatedProbability)
                return items[i];
        }
        return null;    // this code will never come while you use this programm right :)
    }
}

public class Item
{
    public GameObject enemy; // not only string, any type of data
    public int chance;  // chance of getting this Item
}
