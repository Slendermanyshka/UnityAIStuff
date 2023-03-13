using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    float horInput;
    float verInput;

    [Header("Air Stuff")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    float groundDrag;


    Vector3 moveDir;
    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void FixedUpdate()
    {
        InputRead();
        MovePlayer();
        CheckForGround();
    }

    private void InputRead()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verInput + orientation.right * horInput;
        rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);
    }
    private void CheckForGround()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f * 0.2f, whatIsGround);
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
}
