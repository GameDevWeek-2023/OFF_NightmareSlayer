using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject essence;

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
}
