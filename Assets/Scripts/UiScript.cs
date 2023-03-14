using UnityEngine;
using UnityEngine.UI;

public class UiScript : MonoBehaviour
{
    public Image essenzBar;

    public void setEssenz(float count)
    {
        essenzBar.fillAmount = count;
    }
}
