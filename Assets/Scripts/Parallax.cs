using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    private Vector3 previousCamPos;
    private float speed = .3f;
    public Color normalColor;
    public Color nightmareColor;
    public SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        previousCamPos=new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if(previousCamPos!=new Vector3())
        {
            
            transform.position += (cam.transform.position-previousCamPos)*speed;
        }
        previousCamPos = cam.transform.position;
        
    }
    public void SwitchColor()
    {
        foreach(SpriteRenderer renderer in spriteRenderers)
        {
            renderer.color = GameManager.instance.nightmareMode ? nightmareColor : normalColor;
        }
    }
}
