using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    private float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool isAttacking;

    public bool isHealing;

    bool continueAttacking;

    int nextAttack = 0;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    public KeyCode healKey = KeyCode.E;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    PlayerStatus playerStatus;
    Vector3 moveDirection;

    Rigidbody rb;

    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Jumping
    }

    public MovementState movementState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        isHealing = false;

        playerStatus = GetComponent<PlayerStatus>();
    }

    private void ShopVerifier()
    {
        GameObject shopCanvas = GameObject.Find("ShopCanvas");

        if (Vector3.Distance(transform.position, GameObject.Find("Shop").transform.position) < 10)
        {
            if (!shopCanvas.transform.GetChild(1).gameObject.activeSelf)
            {
                shopCanvas.transform.GetChild(0).gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject.Find("Shop").GetComponent<ShopManager>().UpdateMoney();
                shopCanvas.transform.GetChild(0).gameObject.SetActive(false);
                shopCanvas.transform.GetChild(1).gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                shopCanvas.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            shopCanvas.transform.GetChild(0).gameObject.SetActive(false);
            shopCanvas.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, ground);

        if(Time.timeScale != 0)
            MyInput();
        SpeedControl();
        StateControler();
        ShopVerifier();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (!isAttacking && !isHealing)
            MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded && !isAttacking && !isHealing)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        if(Input.GetKey(healKey) && !isAttacking && !isHealing && readyToJump && grounded)
        {
            animator.SetBool("isHealing", true);

            isHealing = true;

            playerStatus.Heal(10);
        }

        if(Input.GetMouseButton(0) && !isHealing)
        {
            animator.SetBool("isAttacking", true);
            isAttacking = true;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && !animator.IsInTransition(0)
                && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                continueAttacking = true;
            }
            else
            {
                continueAttacking = false;
            }
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void StateControler()
    {
        if(grounded)
            animator.SetBool("isJumping", false);

        if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && continueAttacking)
            {
                continueAttacking = false;
                isAttacking = true;
                animator.SetBool("isAttacking", true);

                if (nextAttack == 0 || nextAttack == 1)
                {
                    nextAttack = 2;
                    animator.SetInteger("nextAttack", 2);
                }
                else if (nextAttack == 2)
                {
                    nextAttack = 3;
                    animator.SetInteger("nextAttack", 3);
                }
                else if (nextAttack == 3)
                {
                    nextAttack = 1;
                    animator.SetInteger("nextAttack", 1);
                }
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0) && !continueAttacking)
            {
                isAttacking = false;
                continueAttacking = false;
                nextAttack = 0;
                animator.SetBool("isAttacking", false);
                animator.SetInteger("nextAttack", 0);
            }
        }
        else if(isHealing && animator.GetCurrentAnimatorStateInfo(0).IsTag("Heal"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                isHealing = false;
                animator.SetBool("isHealing", false);
            }
        }
        else if(grounded && Input.GetKey(sprintKey))
        {
            movementState = MovementState.Sprinting;
            moveSpeed = sprintSpeed;
            animator.SetBool("isSprinting", true);
            animator.SetBool("isWalking", true);
        }
        else if (grounded && (horizontalInput != 0 || verticalInput != 0))
        {
            movementState = MovementState.Walking;
            moveSpeed = walkSpeed;
            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", false);
        }
        else if (grounded && horizontalInput == 0 && verticalInput == 0)
        {
            movementState = MovementState.Idle;
            moveSpeed = 0;
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
        }
        else if (!grounded && !readyToJump)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
            movementState = MovementState.Jumping;
            moveSpeed = walkSpeed;
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        readyToJump = true;
    }
}