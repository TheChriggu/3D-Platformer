using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    float verticalLimit = 85;
    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void Rotate(Vector2 rotation)
    {
        transform.Rotate(new Vector3( -rotation.y ,0,0));

        float xAngle = transform.eulerAngles.x;
        if (xAngle > 180) xAngle -= 360;
        if(xAngle > verticalLimit)
        {
            transform.eulerAngles = new Vector3(verticalLimit, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if(xAngle < -verticalLimit)
        {
            transform.eulerAngles = new Vector3(-verticalLimit, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        transform.Rotate(new Vector3(0,rotation.x,0), Space.World);
        if (transform.eulerAngles.z != 0) transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    public void SetLookDirection(Vector3 direction)
    {
        var horizontalRotation = Vector3.Angle(Vector3.forward, new Vector3(direction.x, 0, direction.z));
        if(direction.x < 0) horizontalRotation *= -1;
        var verticalRotation = Vector3.Angle(new Vector3(direction.x, 0, direction.z), direction);
        if (direction.y > 0) verticalRotation *= -1;

        transform.eulerAngles = new Vector3(verticalRotation, horizontalRotation, 0);
    }

    public void LimitRotation(CameraLimits limits)
    {
        float xAngle = transform.eulerAngles.x;
        if(xAngle > limits.maxVerticalRotation)
        {
            transform.eulerAngles = new Vector3(limits.maxVerticalRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if(xAngle < limits.minVerticalRotation)
        {
            transform.eulerAngles = new Vector3(limits.minVerticalRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        float yAngle = transform.eulerAngles.y;
        if(yAngle > limits.maxHorizontalRotation) transform.eulerAngles = new Vector3(transform.eulerAngles.x, limits.maxHorizontalRotation, transform.eulerAngles.z);
        if(yAngle < limits.minHorizontalRotation) transform.eulerAngles = new Vector3(transform.eulerAngles.x, limits.minHorizontalRotation, transform.eulerAngles.z);

        if (transform.eulerAngles.z != 0) transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    public void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
