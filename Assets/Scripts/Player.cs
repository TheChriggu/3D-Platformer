using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject spawningPoint;

    public float speed = 1.0f;
    public float jumpSpeed = 0.15f;
    public float airControlFactor = 0.2f;
    public float gravity = 200;
    public float friction = 2;
    public float drag = 0.05f;
    Vector3 velocity = Vector3.zero;
    Vector3 groundVelocity = Vector3.zero;
    Vector3 input = Vector3.zero;
    float vSpeed = 0;
    bool canDoubleJump = false;



    public GameObject cam;
    Vector3 directionForward;
    public CharacterController controller;
    public Animator anim;




    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Respawn();
    }

    void Update()
    {
        FetchInputDirection();

        if (controller.isGrounded)
        {
            GroundMovement();
        }
        else
        {
            AirMovement();
        }

        vSpeed -= gravity * Time.deltaTime;
        velocity.y = vSpeed;

        ApplyMovement();

    }

    void CalcForwardDirection()
    {
        directionForward = cam.transform.forward;
        directionForward.y = 0;

        directionForward.Normalize();
    }

    void FetchInputDirection()
    {
        CalcForwardDirection();

        input = Input.GetAxis("Vertical") * directionForward
                    + Input.GetAxis("Horizontal") * (Quaternion.AngleAxis(90, Vector3.up) * directionForward);

        input.Normalize();
    }

    void ApplyMovement()
    {
        if(input.magnitude > 0) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(input), 0.1f);
        
        var horizontalVelocity = (velocity - groundVelocity);
        horizontalVelocity.y = 0;
        anim.SetFloat("MovementSpeed", horizontalVelocity.magnitude);
        controller.Move(velocity * Time.deltaTime);
    }

    void GroundMovement()
    {
        anim.SetBool("OnGround", true);
        canDoubleJump = true;

        velocity.x = input.x * speed;
        velocity.z = input.z * speed;
        
        vSpeed = -1;

        if (Input.GetButtonDown("Jump"))
        {
            vSpeed = jumpSpeed;
        }
        else
        {
            if (velocity.x > 0) velocity.x -= friction;
            if (velocity.z > 0) velocity.z -= friction;
            if (velocity.x < 0) velocity.x += friction;
            if (velocity.z < 0) velocity.z += friction;
        }

        groundVelocity = GetFloorMovement();
        velocity += groundVelocity; 
    }

    Vector3 GetFloorMovement()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2, 1 << 8))
        {
            return hit.collider.GetComponent<IMovingGeometry>().Velocity();
        }
        else
        {
            return Vector3.zero;
        }
    }

    void AirMovement()
    {
        anim.SetBool("OnGround", false);
        groundVelocity = Vector3.zero;

        Vector3 currentDirection = velocity;
        currentDirection.y = 0;
        currentDirection.Normalize();
        velocity -= currentDirection * drag;

        Vector3 movementCorrection = input * speed * airControlFactor;
        velocity -= currentDirection * movementCorrection.magnitude;
        velocity += movementCorrection;

        if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            vSpeed = jumpSpeed;
            canDoubleJump = false;
        }
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = spawningPoint.transform.position;
        transform.rotation = spawningPoint.transform.rotation;

        velocity = Vector3.zero;
        groundVelocity = Vector3.zero;
        input = Vector3.zero;
        vSpeed = 0;
        canDoubleJump = false;

        controller.enabled = true;

        FindObjectOfType<CameraController>().Reset();
    }
}
