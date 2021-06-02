using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewFirstScene : PlayerView
{
    [SerializeField] private Transform playerHead, cameraTransform;
    [SerializeField] private Vector2 minPitch, maxPitch;
    private Vector2 cameraPitchVector;

    protected override void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseVelocity, mouseSmoothTime);

        cameraPitchVector += currentMouseDelta * mouseSensitivity;
        cameraPitchVector.x = Mathf.Clamp(cameraPitchVector.x, minPitch.y, maxPitch.y);
        cameraPitchVector.y = Mathf.Clamp(cameraPitchVector.y, minPitch.x, maxPitch.x);

        playerHead.localEulerAngles = Vector3.up * cameraPitchVector.x;
        cameraTransform.localEulerAngles = Vector3.right * -cameraPitchVector.y;

        cameraTransform.position = cameraOffSet.transform.position;
        cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x, playerHead.localEulerAngles.y, 0);
    }
}