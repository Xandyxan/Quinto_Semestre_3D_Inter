using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] bool lookCursor = true;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.3f;
    
    private CharacterController characterController;
    private Animator animator;

    [Header("Ground Check stuff")]
    [SerializeField] float gravity = -13.0f;
    float velocityY = 0.0f;

    float cameraPitch = 0f;
    public float walkSpeedX, actualWalkSpeedZ;
    float walkSpeedZ = 2.0f;
    float runSpeedZ = 4.0f;
    float backWalkSpeedZ = 1.25f;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    private Vector3 velocityC = Vector3.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseVelocity = Vector2.zero;
    [SerializeField] Transform head;

    [Header("Zoom")]
    public Camera mainCamera;


    public Image crosshair;
    private float chRaio = 50f;

    private float fovDefault = 90f;
    private float fovZoom = 50f;
    private float zoomSpeed = 8f;

    //run
    private float fovRun = 80f;
    private bool isRunning;
    private Inspecao insp;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        actualWalkSpeedZ = walkSpeedZ;

    }

    // Start is called before the first frame update
    void Start()
    {
         insp = FindObjectOfType<Inspecao>();
        if(lookCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
         if(insp.inspecionando == false)
        {
            UpdateMouseLook();
            UpdateMovement();    
        }
        HandleZoom();
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseVelocity, mouseSmoothTime);
        //print(currentMouseDelta);
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        head.localEulerAngles = Vector3.right * cameraPitch;
        //playerCamera.Rotate(Vector3.right * -(mouseDelta.y * mouseSensitivity), Space.Self);
    }

    private void UpdateMovement()
    {
        isRunning = false;
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (currentDir.y > -0.0001f && currentDir.y < 0.0001f) actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, 0, Time.deltaTime * 10f);
        else if (currentDir.y <= 0f) actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, backWalkSpeedZ, Time.deltaTime * 10f);
        else if (currentDir.y > 0f && !Input.GetButton("Run")) actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, walkSpeedZ, Time.deltaTime * 10f);
        else if (currentDir.y < 0f && actualWalkSpeedZ <= -1.4f) actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, walkSpeedZ, Time.deltaTime * 1f);
        else if (currentDir.y > -0.2f && Input.GetButton("Run"))
        {
            actualWalkSpeedZ = Mathf.Lerp(actualWalkSpeedZ, runSpeedZ, Time.deltaTime * 2f);
            isRunning = true;
        }


        if (characterController.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y * actualWalkSpeedZ) + (transform.right * currentDir.x * walkSpeedX) + Vector3.up * velocityY;
        animator.SetFloat("Velocity", Input.GetAxis("Vertical") * actualWalkSpeedZ);
        //print(currentDir.y);

        //When a GameObject is rotated, the blue arrow representing the Z axis of the GameObject also changes direction.
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleZoom()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, fovZoom, zoomSpeed * Time.deltaTime);
            // make crosshair smaller.
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaio / 2, chRaio / 2), zoomSpeed * Time.deltaTime);
        }else if (isRunning)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, fovRun, zoomSpeed * Time.deltaTime);
        }
        else if (mainCamera.fieldOfView != fovDefault)
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, fovDefault, (zoomSpeed - 2) * Time.deltaTime);
            // return crosshair to bigger size.
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaio, chRaio), (zoomSpeed - 2) * Time.deltaTime);
        }
    }

}
