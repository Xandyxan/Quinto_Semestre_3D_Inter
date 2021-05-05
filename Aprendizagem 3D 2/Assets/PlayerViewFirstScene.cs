using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewFirstScene : PlayerView
{
    [SerializeField] private Transform playerHead, cameraTransform;
    [SerializeField] private Vector2 minPitch, maxPitch;
    private new Vector2 cameraPitch;

    protected override void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseVelocity, mouseSmoothTime);

        cameraPitch += currentMouseDelta * mouseSensitivity;
        cameraPitch.x = Mathf.Clamp(cameraPitch.x, minPitch.y, maxPitch.y);
        cameraPitch.y = Mathf.Clamp(cameraPitch.y, minPitch.x, maxPitch.x);

        playerHead.localEulerAngles = Vector3.up * cameraPitch.x;
        cameraTransform.localEulerAngles = Vector3.right * -cameraPitch.y;

        cameraTransform.position = cameraOffSet.transform.position;
        cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x, playerHead.localEulerAngles.y, 0);
    }
}