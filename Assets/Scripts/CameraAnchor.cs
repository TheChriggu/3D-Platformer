using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 targetPosition;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        targetPosition = transform.localPosition;
    }

    void Update()
    {
        CheckForViewBlockage();
    }

    void CheckForViewBlockage()
    {
        RaycastHit hit;

        Vector3 origin = transform.parent.position;
        Vector3 direction = transform.TransformPoint(targetPosition) - origin;

        if(Physics.Raycast(origin, direction, out hit, direction.magnitude))
        {
            var position = transform.localPosition;
            position.z = -hit.distance;
            transform.localPosition = position;
        }
        else
        {
            transform.localPosition = targetPosition;
        }
    }

    public void SetDistance(float distance)
    {
        targetPosition.z = -distance;
        if (targetPosition.z > -1) targetPosition.z = -1;
    }

    public void SetOffset(Vector2 offset)
    {
        targetPosition.x = offset.x;
        targetPosition.y = offset.y;
    }

    public void Zoom(float distance)
    {
        SetDistance(-targetPosition.z - distance);
    }

    public void LimitDistance(CameraLimits limits)
    {
        if(targetPosition.z < -limits.maxDistance) targetPosition.z = -limits.maxDistance;
        if(targetPosition.z > -limits.minDistance) targetPosition.z = -limits.minDistance;
    }

    public void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        targetPosition = transform.localPosition;
    }
}
