
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewEnemyDropLoot", menuName = "Custom Scriptables/Enemy Drop Loot" )]
public class DropChances : ScriptableObject
{
    public List<ItemChance> Items;


    public GameObject GetDrop()
    {
        int maxChances=0;
        foreach (ItemChance i in Items)
        {
            maxChances += i.chance;
        }

        int roll = Random.Range(0, maxChances+1);
        int chanceSum = 0;
        
        foreach (ItemChance i in Items)
        {
            chanceSum += i.chance;
            if(roll>=chanceSum) continue;
//            Debug.Log(i.Item + " - Droped from roll: " +roll);
            return i.Item;
            break;
        }
        
        
        return null;
    }
}

[Serializable]
public struct ItemChance
{
    public GameObject Item;
    public int chance;
}