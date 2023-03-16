using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchNightmareEditor
{
    [MenuItem("Nightmare Mode/Switch Nightmare")]
    private static void SwitchNightmare()
    {
        GameObject[] objectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        SwitchSprites(objectsInScene);
    }

    private static void SwitchSprites(GameObject[] objects)
    {
        foreach (var gameObject in objects)
        {
            SwitchableObject switchable = gameObject.GetComponent<SwitchableObject>();
            if(switchable != null) switchable.SwitchObject();

            GameObject[] children = new GameObject[gameObject.transform.childCount];
            
            for (var i = 0; i < children.Length; i++)
            {
                children[i] = gameObject.transform.GetChild(i).gameObject;
            }

            SwitchSprites(children);
        }
    }
}
