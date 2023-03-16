using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool grounded;
    bool readyTojump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public float jumpForce;
    public float jumpCooldown;
    public float airMult;
    float horInput;
    float verInput;

    [Header("Air Stuff")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundDrag;
    


    Vector3 moveDir;
    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }
    void FixedUpdate()
    {
        InputRead();
        MovePlayer();
        ControlSpeed();
        CheckForGround();
    }

    private void InputRead()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyTojump && grounded)
        {
            readyTojump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verInput + orientation.right * horInput;
        rb.AddForce(moveDir.normalized * moveSpeed*10f, ForceMode.Force);
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
    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x,0f ,rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyTojump = true;
    }
}
