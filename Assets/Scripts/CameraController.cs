using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraLimits
{
    public float minVerticalRotation;
    public float maxVerticalRotation;
    public float minHorizontalRotation;
    public float maxHorizontalRotation;
    public float minDistance;
    public float maxDistance;
}

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        Free = 0,
        Limited,
        Fixed
    }

    public CameraMode cameraMode = CameraMode.Free;

    public CameraFocus focus;
    public Vector3 focusOffset;
    public CameraAnchor anchor;
    public Camera cam;
    public Player player;

    public float rotationalSpeed = 10;
    public float zoomSpeed = 10;
    public float movementSpeed = 5;
    

    Vector3 fixedAnchorPosition = new Vector3 (5,10,5);
    CameraLimits limits;

    void Start()
    {
        TeleportCameraToPlayer();
        Cursor.visible = false;
    }

    void Update()
    {
        focus.SetPosition(player.transform.position + focusOffset);
        
        switch(cameraMode)
        {
            case CameraMode.Free:
                UpdateFreeCamera();
                break;
            case CameraMode.Fixed:
                UpdateFixedCamera();
                break;
            case CameraMode.Limited:
                UpdateLimitedCamera();
                break;
            default:
                UpdateFixedCamera();
                break;
        }

        MoveCamToAnchor();
    }

    void UpdateFreeCamera()
    {
        focus.Rotate(InputRotation() * rotationalSpeed * Time.deltaTime);
        anchor.Zoom(InputZoom() * zoomSpeed);
    }

    void UpdateLimitedCamera()
    {
        UpdateFreeCamera();
        focus.LimitRotation(limits);
        anchor.LimitDistance(limits);
    }

    void UpdateFixedCamera()
    {
        focus.SetLookDirection(focus.transform.position - fixedAnchorPosition);
        anchor.SetDistance(Vector3.Distance(focus.transform.position, fixedAnchorPosition));
    }

    Vector2 InputRotation()
    {
        var mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        return mouseInput;
    }

    float InputZoom()
    {
        var zoom = Input.GetAxis("Mouse ScrollWheel");
        if(zoom == 0)
        {
            if(Input.GetButton("ZoomIn")) zoom = Time.deltaTime;
            else if(Input.GetButton("ZoomOut")) zoom = -Time.deltaTime;
        }
        return zoom;
    }

    void MoveCamToAnchor()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, anchor.transform.position, movementSpeed * Time.deltaTime);
        cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, anchor.transform.rotation, rotationalSpeed * Time.deltaTime);
    }

    public void FixCameraToPosition(Vector3 position)
    {
        fixedAnchorPosition = position;
        cameraMode = CameraMode.Fixed;
    }

    public void FreeCamera()
    {
        cameraMode = CameraMode.Free;
    }

    public void LimitCameraTo(CameraLimits newLimits)
    {
        limits = newLimits;
        cameraMode = CameraMode.Limited;
    }

    void TeleportCameraToPlayer()
    {
        focus.SetPosition(player.transform.position + focusOffset);

        cam.transform.position = anchor.transform.position;
        cam.transform.rotation = anchor.transform.rotation;
    }

    public void Reset()
    {
        cameraMode = CameraMode.Free;
        focus.Reset();
        anchor.Reset();
        TeleportCameraToPlayer();
    }
}
