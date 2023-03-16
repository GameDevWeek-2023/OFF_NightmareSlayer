using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public TextMeshProUGUI textField;
    private int stage;
    private bool WASDLock = false;
    private bool JumpLock = false;
    private bool AttackLock = false;
    private bool played = false;
    

    // Update is called once per frame
    private void OnEnable()
    {
        if (stage == 0) WASDLock = false;
        if (stage == 1) JumpLock = false;
        if (stage == 2) AttackLock = false;
    }

    private void Awake()
    {
        played = PlayerPrefs.GetInt("TutorialPlayed") == 1;
        if (!played) textField.text = "Press A/D to move sideways";
        else
        {
            ende();
        }
    }

    void Update()
    {
        
            switch (stage)
            {
                case 0:
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        if(WASDLock == false)StartCoroutine(walking());
                    }
                    break;
                case 1:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        if (JumpLock == false) StartCoroutine(jumping());
                    }
                    break;
                case 2:
                    break;
            }
        
    }


    IEnumerator walking()
    {
        WASDLock = true;

        yield return new WaitForSeconds(2);
        textField.text = "To jump press Space";
        stage = 1;
    }

    IEnumerator jumping()
    {
        JumpLock = true;
        yield return new WaitForSeconds(2);
        textField.text = "To attack use Left-Click";
        stage = 2;
    }

    private void ende()
    {
        played = true;
        PlayerPrefs.SetInt("TutorialPlayed", 1);
        stage = 100;
        gameObject.SetActive(false);
    }
}
