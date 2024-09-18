using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Get the main camera's transform
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Make the object face the camera by setting its forward direction
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
