using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _cameraToLookAt;
   
    private void Start()
    {
 
        _cameraToLookAt = Camera.main;
    }

    private void LateUpdate()
    {

        transform.LookAt(transform.position + _cameraToLookAt.transform.rotation * Vector3.forward,
            _cameraToLookAt.transform.rotation * Vector3.up);
        
    }
}
