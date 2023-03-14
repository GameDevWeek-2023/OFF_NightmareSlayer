using UnityEngine;
using UnityEngine.UI;

public class EssenzManager : MonoBehaviour
{
    public Image essenzBar;

    public void SetEssenz(float count)
    {
        essenzBar.fillAmount = count;
    }
}
