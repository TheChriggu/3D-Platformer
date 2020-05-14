using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraTrigger : MonoBehaviour
{
    CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cameraController.FreeCamera();
        }
    }
}
