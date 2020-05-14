using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedCameraTrigger : MonoBehaviour
{    public CameraLimits limits;
    CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cameraController.LimitCameraTo(limits);
        }
    }
}
