using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cameraToLookAt;
   
    private void Start()
    {
 
        cameraToLookAt = Camera.main;
    }

    private void LateUpdate()
    {

        transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.forward,
            cameraToLookAt.transform.rotation * Vector3.up);
        
    }
}
