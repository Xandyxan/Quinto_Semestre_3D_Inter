﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    [Header("Ground Check stuff")]
    [SerializeField] float gravity = -13.0f;
    float velocityY = 0.0f;

    public float walkSpeedX, actualWalkSpeedZ;
    float walkSpeedZ = 2.0f;
    float runSpeedZ = 4.0f;
    float backWalkSpeedZ = 1.25f;
    float crouchSpeedZ = 1f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    //Character States
    private bool isRunning, isCrouched;

    [Header("Other")]
    [SerializeField] SelectionManager SelectionManager;

    private bool usingCellphone;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        actualWalkSpeedZ = walkSpeedZ;
    }
    
    private void OnEnable()
    {
        //Cellphone.instance.usingCellphoneEvent -= TurnPlayerControllerOff; // we remove the methods from the delegate at the beggining to prevent it to run multiple times.
        //Cellphone.instance.closeCellMenuEvent -= TurnPlayerControllerOn;
        //Cellphone.instance.usingCellphoneEvent += TurnPlayerControllerOff;
        //Cellphone.instance.closeCellMenuEvent += TurnPlayerControllerOn;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    void Update()
    {
       // UpdateMovement();

        
        if (!usingCellphone)
        {
            if (!SelectionManager.inspecionando)
            {
                UpdateMovement();
            }
            
        }
        else   // disable the script only after the player interpolates to the idle animation.
        {
            actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, 0, Time.deltaTime * 6f);
            animator.SetFloat("Velocity", actualWalkSpeedZ);
            isRunning = false;
            currentDir = Vector2.zero;
            if (actualWalkSpeedZ < .1f)
            {
                this.enabled = false;
               // print("PCtrl OF");
            }
        }
        
    }
    private void UpdateMovement()
    {
        InputManager();

        //Get RawMovement and normalize
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        
        //Pass raw values to make a smooth transtion
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        SmoothWalkSpeedZ();

        if (characterController.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y * actualWalkSpeedZ) + (transform.right * currentDir.x * walkSpeedX) + Vector3.up * velocityY;
        
        characterController.Move(velocity * Time.deltaTime);
        SetAnimators();
    }

    private void InputManager()
    {
        if(Input.GetAxis("Horizontal") >= 0 || Input.GetAxis("Vertical") >=0)
        {
                UpdateCollider();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (isCrouched) isCrouched = false;
            else isCrouched = true;
            isRunning = false;
        }
        

        if (Input.GetButton("Run"))
        {
            isRunning = true;
            isCrouched = false;
        }
        else isRunning = false;

        if(Input.GetAxisRaw("Vertical") <= 0f) isRunning = false;
    }

    private void SmoothWalkSpeedZ() //It's to make velocity changes in a smooth way getting the smooth currentDir
    {
        if (!isCrouched && !isRunning)
        {
            if (currentDir.y > -0.0001f && currentDir.y < 0.0001f) //From any state to -> To idle stand state
                actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, 0, Time.deltaTime * 10f);

            else if (currentDir.y <= 0f) //From any state to -> To walkingBack stand state
                actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, backWalkSpeedZ, Time.deltaTime * 10f);

            else if (currentDir.y > 0f) //From any state to -> To walk forward stand state
                actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, walkSpeedZ, Time.deltaTime * 10f);
        }
        else if (isRunning && !isCrouched)
        {
            if (currentDir.y > 0f) //Se está correndo em pé
                actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, runSpeedZ, Time.deltaTime * 2f);
        }
        else if (isCrouched && !isRunning)
        {
            if (currentDir.y > -0.0001f && currentDir.y < 0.0001f) //From any state to -> To idle crouch state
                actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, 0, Time.deltaTime * 50f);

            else if (currentDir.y > 0f && isCrouched && !isRunning) //From any state to -> To walk crouch state
                actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, crouchSpeedZ, Time.deltaTime * 50f);
        }
    }

    private void SetAnimators()
    {
        animator.SetFloat("Velocity", Input.GetAxis("Vertical") * actualWalkSpeedZ);
        animator.SetBool("isCrouched", isCrouched);
    }

    private void UpdateCollider()
    {
        if(!isCrouched)
        {
            characterController.center = new Vector3(0, 0.0775f, 0);
            characterController.radius = 0.03f;
            characterController.height = 0.155f;
        }
        else
        {
            characterController.center = new Vector3(0, 0.0775f, 0.02f);
            characterController.radius = 0.061f;
            characterController.height = 0.155f;
        }
    }

    //getters
    public bool GetIsRunning() { return this.isRunning; }
    public bool GetIsWalking()
    {
        if (this.currentDir.y >= 0.05f) return true;
        else return false;
    }

    // On and Off
    public void TurnPlayerControllerOn()
    {
        usingCellphone = false;
        this.enabled = true;
       // print(" PCtrl ON");
    }
    public void TurnPlayerControllerOff() 
    {
        usingCellphone = true;
    }
}
