using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVision : MonoBehaviour
{
    [SerializeField] private Transform playerHead, cameraTransform, cameraOffSet;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] bool lookCursor = true;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.3f;
    private Vector2 cameraPitch = Vector2.zero;
    [SerializeField] private Vector2 minPitch, maxPitch;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseVelocity = Vector2.zero;

    [Header("Zoom")]
    private Camera cameraPlayer;
    [SerializeField] private Image crosshair;
    [SerializeField] private float defaultZoomSpeed;
    private float chRaio = 50f;

    private float fovDefault = 90f;
    private float fovZoom = 50f;
    private float zoomSpeed = 8f;

    private void Awake()
    {
        cameraPlayer = cameraTransform.GetComponent<Camera>();
    }

    void Start()
    {
        if (lookCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMouseLook();
        HandleZoom();
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseVelocity, mouseSmoothTime);


        cameraPitch += currentMouseDelta * mouseSensitivity;
        cameraPitch.x = Mathf.Clamp(cameraPitch.x, minPitch.y, maxPitch.y);
        cameraPitch.y = Mathf.Clamp(cameraPitch.y, minPitch.x, maxPitch.x);

        //playerCamera.Rotate(Vector3.right * currentMouseDelta.y * mouseSensitivity + Vector3.up * currentMouseDelta.x * mouseSensitivity);
        playerHead.localEulerAngles = Vector3.up * cameraPitch.x;
        cameraTransform.localEulerAngles = Vector3.right * -cameraPitch.y;


        cameraTransform.position = cameraOffSet.transform.position;
        cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x, playerHead.localEulerAngles.y, 0);
    }

    private void HandleZoom()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            cameraPlayer.fieldOfView = Mathf.Lerp(cameraPlayer.fieldOfView, fovZoom, defaultZoomSpeed * Time.deltaTime);
            // make crosshair smaller.
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaio / 2, chRaio / 2), zoomSpeed * Time.deltaTime);
        }
        else if (cameraPlayer.fieldOfView != fovDefault)
        {
            cameraPlayer.fieldOfView = Mathf.Lerp(cameraPlayer.fieldOfView, fovDefault, defaultZoomSpeed * Time.deltaTime);
            // return crosshair to bigger size.
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, new Vector2(chRaio, chRaio), (zoomSpeed - 2) * Time.deltaTime);
        }
    }

}
