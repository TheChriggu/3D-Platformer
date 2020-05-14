using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraTrigger : MonoBehaviour
{
    public GameObject cameraPosition;
    CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cameraController.FixCameraToPosition(cameraPosition.transform.position);
        }
    }
}
