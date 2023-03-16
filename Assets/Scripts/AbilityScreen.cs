using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityScreen : MonoBehaviour
{
    public Image abilityImage;
    public TextMeshProUGUI abilityTitle;
    public TextMeshProUGUI abilityDescription;

    public void SetUpScreen(Sprite icon, string title, string description)
    {
        abilityImage.sprite = icon;
        abilityTitle.text = title;
        abilityDescription.text = description;
    }
}
