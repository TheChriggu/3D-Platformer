using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour, IMovingGeometry
{
    public Vector3 moveTowards =  Vector3.forward;
    Vector3 relativeTurningPoint;
    public float distance = 10;
    float distanceToMove;
    public float speed = 30;
    public float waitTime = 5.0f;
    bool canMove = true;
    float arrivalThreshhold = 0.05f;

    void Start()
    {
        distanceToMove = distance;
        relativeTurningPoint = moveTowards;
    }

    void Update()
    {
        if(canMove)
        {
            var direction = relativeTurningPoint.normalized;
            var movementVector = direction*speed*Time.deltaTime;
            transform.Translate(movementVector);
            relativeTurningPoint -= movementVector;

            if(relativeTurningPoint.magnitude <= arrivalThreshhold)
            {
                moveTowards *= -1;
                relativeTurningPoint = moveTowards;
                StartCoroutine("Wait");
            }
        }
    }

    IEnumerator Wait()
    {
        canMove = false;
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }

    public Vector3 Velocity()
    {
        if(!canMove) return Vector3.zero;

        return (transform.localToWorldMatrix*relativeTurningPoint).normalized * speed;
    }
}
