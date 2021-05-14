using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    protected Camera playerCamera;
    protected Transform playerCameraTransform;
    [SerializeField] protected Transform cameraOffSet;

    [SerializeField] protected bool lookCursor = true;
    [SerializeField] protected float mouseSensitivity = 5f;
    [SerializeField] [Range(0.0f, 0.5f)] protected float mouseSmoothTime = 0.05f;

    protected float cameraPitch = 0f;
    protected Vector2 currentMouseDelta = Vector2.zero;
    protected Vector2 currentMouseVelocity = Vector2.zero;

    [Header("Zoom")]
    [SerializeField] private Image crosshair;
    [SerializeField] private float defaultZoomSpeed = 8f;
    protected float chRaio = 50f;
    protected float fovDefault = 60f;   // Vertical fov value, the horizontal one is based on the screen resolution
    protected float fovZoom = 40f;
    protected float zoomSpeed = 8f;

    [Header("Other")]
    [SerializeField] SelectionManager SelectionManager;
    private bool usingCellphone;

    [Header("Crouching")]
    private bool isCrouching;

    private void Awake()
    {
        playerCamera = FindObjectOfType<Camera>();
        playerCameraTransform = playerCamera.GetComponent<Transform>();

        playerCameraTransform.position = cameraOffSet.transform.position;
    }

    
    private void OnEnable()
    {

    }
    void Start()
    {
        if (lookCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        GameManager.instance.removePlayerControlEvent -= TurnPlayerVisionOff; // we remove the methods from the delegate at the beggining to prevent it to run multiple times.
        GameManager.instance.returnPlayerControlEvent -= TurnPlayerVisonOn;
        GameManager.instance.removePlayerControlEvent += TurnPlayerVisionOff;
        GameManager.instance.returnPlayerControlEvent += TurnPlayerVisonOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (!usingCellphone)
        {
            if (!SelectionManager.inspecionando)
            {
                UpdateMouseLook();
            }
            HandleZoom();
        }
        
       
    }

    protected virtual void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseVelocity, mouseSmoothTime);
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);


        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        
        cameraPitch = isCrouching? Mathf.Clamp(cameraPitch, -60.0f, 90.0f) : Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCameraTransform.localEulerAngles = Vector3.right * cameraPitch;

        //this is the "HeadBobber", cameraOffSet is inside of head rig animation
        playerCameraTransform.position = cameraOffSet.transform.position;
        
    }

    private void HandleZoom()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fovZoom, defaultZoomSpeed * Time.deltaTime);
            // make crosshair smaller.
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaio / 2, chRaio / 2), zoomSpeed * Time.deltaTime);
        }
        else if (playerCamera.fieldOfView != fovDefault)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fovDefault, defaultZoomSpeed * Time.deltaTime);
            // return crosshair to bigger size.
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaio, chRaio), (zoomSpeed - 2) * Time.deltaTime);
        }
    }

    // On and Off
    public void TurnPlayerVisonOn()
    {
        usingCellphone = false;
        this.enabled = true;
        // print(" PCtrl ON");
    }
    public void TurnPlayerVisionOff()
    {
        usingCellphone = true;
    }
    // Setters
    public void SetIsCrouching(bool value)
    {
        isCrouching = value;
    }
}
