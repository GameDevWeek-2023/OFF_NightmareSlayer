using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchNightmareEditor
{
    private static bool isNightmare = false;
    
    [MenuItem("Nightmare Mode/Switch Nightmare")]
    private static void SwitchNightmare()
    {
        isNightmare = !isNightmare;
        GameObject[] objectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        SwitchSprites(objectsInScene);
    }

    private static void SwitchSprites(GameObject[] objects)
    {
        foreach (var gameObject in objects)
        {
            if(gameObject == null) continue;
            GameObject[] children = new GameObject[gameObject.transform.childCount];
            
            for (var i = 0; i < children.Length; i++)
            {
                children[i] = gameObject.transform.GetChild(i).gameObject;
            }
            
            SwitchableObject switchable = gameObject.GetComponent<SwitchableObject>();
            if(switchable != null) switchable.SwitchObject(isNightmare);
            else
            {
                ReplaceEnemy replaceable = gameObject.GetComponent<ReplaceEnemy>();
                    if(replaceable != null) replaceable.Replace(isNightmare);
                    else
                    {
                        Fruit fruit = gameObject.GetComponent<Fruit>();
                                    if(fruit != null) fruit.SwitchFruit(isNightmare);
                    }
            }
            
            

            SwitchSprites(children);
        }
    }
}
