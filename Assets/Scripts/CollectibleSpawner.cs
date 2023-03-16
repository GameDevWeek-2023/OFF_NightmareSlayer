using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject essence;
    public GameObject coin;

    public void SpawnEssence(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(essence, transform.position, Quaternion.identity);
        }
    }
    
    public void SpawnEssenceAndDie(int amount)
    {
        SpawnEssence(amount);
        Destroy(gameObject);
    }
    
    public void SpawnCoins(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
    }
    
    public void SpawnCoinsAndDie(int amount)
    {
        SpawnCoins(amount);
        Destroy(gameObject);
    }
}
