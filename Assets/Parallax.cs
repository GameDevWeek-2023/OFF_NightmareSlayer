using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    private Vector3 previousCamPos;
    private float speed = .3f;
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
}
