using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private Transform doorToOpen;
    private Quaternion desireRotation, idleRotation;

    [SerializeField] private Vector3 desirePosition;
    private Vector3 idlePosition;

    [SerializeField] private float velocityToOpen;
    private bool openingIsHappening, isOpened;

    [Header("Wich type of movement it is?")]
    [SerializeField] private bool xRotate;
    [SerializeField] private bool yRotateLeft;
    [SerializeField] private bool yRotateRight;
    [SerializeField] private bool zPosition;

    [SerializeField] private Vector3 desireRotationValues;

    private void Awake()
    {
        doorToOpen = this.transform;
        openingIsHappening = isOpened = false;

        idleRotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        idlePosition = this.transform.localPosition;

        DesireAxisRotation();
    }
    private void Start()
    {
        if (velocityToOpen == 0) velocityToOpen = 40f;
    }

    // Update is called once per frame
    void Update()
    {
        if (openingIsHappening) Opening();
    }

    public void OpenCloseDoors() { openingIsHappening = true; } //É chamado no 'Interactive' script

    private void Opening()
    {
        if (xRotate || yRotateLeft || yRotateRight)
        {
            if (!isOpened) doorToOpen.rotation = Quaternion.RotateTowards(doorToOpen.rotation, desireRotation, velocityToOpen * Time.deltaTime);
            else if (isOpened) doorToOpen.rotation = Quaternion.RotateTowards(doorToOpen.rotation, idleRotation, velocityToOpen * Time.deltaTime);
        }
        else if (zPosition)
        {
            if (!isOpened) this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, desirePosition, Time.deltaTime * velocityToOpen);
            else if (isOpened) this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, idlePosition, Time.deltaTime * velocityToOpen);
        }

        CheckDoorState();
    }

    private void CheckDoorState()
    {
        if (xRotate)
        {
            if (doorToOpen.eulerAngles.x <= idleRotation.eulerAngles.x) SetIsOpened(false);
            else if (doorToOpen.eulerAngles.x >= desireRotation.eulerAngles.x) SetIsOpened(true);
        }
        else if (yRotateLeft)
        {
            if (this.transform.rotation.eulerAngles.y <= idleRotation.eulerAngles.y) SetIsOpened(false);
            else if (this.transform.rotation.eulerAngles.y >= desireRotation.eulerAngles.y) SetIsOpened(true);
        }
        else if (yRotateRight)
        {
            if (this.transform.rotation.eulerAngles.y >= idleRotation.eulerAngles.y) SetIsOpened(false);
            else if (this.transform.rotation.eulerAngles.y <= desireRotation.eulerAngles.y) SetIsOpened(true);
        }
        else if (zPosition)
        {
            if (this.transform.localPosition.z <= idlePosition.z + 0.01f) SetIsOpened(false);
            else if (this.transform.localPosition.z >= desirePosition.z - 0.01f) SetIsOpened(true);
        }
    }

    private void DesireAxisRotation()
    {
        if (xRotate) desireRotation = Quaternion.Euler(desireRotationValues.x, transform.eulerAngles.y, transform.eulerAngles.z);
        if (yRotateLeft || yRotateRight) desireRotation = Quaternion.Euler(transform.eulerAngles.x, desireRotationValues.y, transform.eulerAngles.z);
        if (zPosition) desirePosition = new Vector3(this.doorToOpen.localPosition.x, this.doorToOpen.localPosition.y, desirePosition.z);
    }

    private void SetIsOpened(bool isOpened)
    {
        this.isOpened = isOpened;
        if (openingIsHappening) openingIsHappening = false;
    }
}
