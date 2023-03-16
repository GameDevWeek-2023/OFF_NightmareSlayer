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
    }
    void Start()
    {
        collider = GetComponent<Collider2D>();
        
        AddFruit();
    }

    public static void SwitchAllFruit(bool isNightmare)
    {
        if (allFruits == null) return;
        foreach (var current in allFruits)
        {
            current.SwitchFruit(isNightmare);
        }
    }
    
    public void SwitchFruit(bool isNightmare)
    {
        if (isNightmare)
        {
            hasFruit = false;
            UpdateFruit();
        }
        else
        {
            hasFruit = true;
            UpdateFruit();
        }
    }

    private void UpdateFruit()
    {
        if (hasFruit)
        {
            fruit.SetActive(true);
            collider.enabled = true;
        }
        else
        {
            fruit.SetActive(false);
            collider.enabled = false;
        }
    }
    
    public void ObtainFruit()
    {
        switch (fruitType)
        {
            case FruitType.Dream:
                //TODO do effect stuff on dream fruit
                
                //TODO Splash into essence
                
                break;
            case FruitType.Heartberry:
                //TODO do effect stuff on heartberry
                PlayerScript.instance.Heal();
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
}
