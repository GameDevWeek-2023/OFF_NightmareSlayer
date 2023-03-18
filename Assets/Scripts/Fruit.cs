using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public enum FruitType
    {
        Dream,
        Heartberry
    }

    public FruitType fruitType;
    public GameObject fruit;
    private bool hasFruit;

    private Collider2D collider;
    private static List<Fruit> allFruits;

    private void Awake()
    {
        if (allFruits == null) allFruits = new List<Fruit>();
        if (!allFruits.Contains(this)) allFruits.Add(this);
        
        collider = GetComponent<Collider2D>();
    }
    void Start()
    {
        AddFruit();
    }

    public static void SwitchAllFruit(bool isNightmare)
    {
        if (allFruits == null) return;
        foreach (var fruit in allFruits)
        {
            fruit.SwitchFruit(isNightmare);
        }
    }
    
    public void SwitchFruit(bool isNightmare)
    {
        /*if (isNightmare)
        {
            hasFruit = false;
            UpdateFruit();
        }
        else
        {
            hasFruit = true;
            UpdateFruit();
        }*/
        if (!isNightmare)
        {
            AddFruit();
        }
    }

    private void UpdateFruit()
    {
        if (hasFruit)
        {
            fruit.SetActive(true);
            if(collider != null) collider.enabled = true;
        }
        else
        {
            fruit.SetActive(false);
            if(collider != null) collider.enabled = false;
        }
    }
    
    public void ObtainFruit()
    {
        switch (fruitType)
        {
            case FruitType.Dream:
                //TODO do effect stuff on dream fruit
                GetComponent<CollectibleSpawner>().SpawnEssence(15);
                break;
            case FruitType.Heartberry:
                //TODO do effect stuff on heartberry
                PlayerScript.instance.FullHeal();
                break;
        }

        RemoveFruit();
    }
    
    public void AddFruit()
    {
        hasFruit = true;
        UpdateFruit();
    }

    public void RemoveFruit()
    {
        hasFruit = false;
        UpdateFruit();
    }
    
    
    public static void RemoveAll()
    {
        allFruits = new List<Fruit>();
    }
}
