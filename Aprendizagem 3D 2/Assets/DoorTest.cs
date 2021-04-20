using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
    public float desireY;
    public bool openingIsHappening, isOpened;
    private Vector3 idleRotation;

    private bool rotateRigth, rotateLeft;

    //89.99001 = -0.01f no inspector

    private void Awake()
    {
        isOpened = rotateRigth = rotateLeft = false;

        if (desireY > 1f) rotateLeft = true;
        if (desireY < -1f) rotateRigth = true;

        idleRotation = this.transform.localEulerAngles;
    }
    // Start is called before the first frame update
    void Start()
    {
        print(transform.rotation.eulerAngles.y);
        print(transform.localEulerAngles.y);
        print(idleRotation.y);
    }



    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(transform.position, -transform.up, 20 * Time.deltaTime);
        //Opening();
        if (openingIsHappening) Opening();
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, UnwrapAngle(-90), 0), 2 * Time.deltaTime);


        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, UnwrapAngle(0.01f), transform.localEulerAngles.z);

        
    }

    public void OpenCloseDoors() { openingIsHappening = true; } //É chamado no 'Interactive' script

    private void Opening()
    {
        if (rotateLeft)
        {
            if (isOpened) transform.RotateAround(transform.position, -transform.up, 20 * Time.deltaTime);
            else if (!isOpened) transform.RotateAround(transform.position, transform.up, 20 * Time.deltaTime);
        }
        CheckDoorState();
    }

    private void CheckDoorState()
    {
        if(rotateRigth)
        {
            if (transform.localEulerAngles.y <= desireY) SetIsOpened(false);
            else SetIsOpened(true);
        }

        if (rotateLeft)
        {
            if (transform.localEulerAngles.y >= desireY) SetIsOpened(true);
            //else if (transform.localEulerAngles.y <= idleRotation.y - 1) SetIsOpened(false);
            if (transform.eulerAngles.y <= 180.01f)
            {
                print("Atingiu");
                transform.localEulerAngles = new Vector3(0, 0.01f, 0);
                //if (transform.rotation.eulerAngles.y <= 0.009999274f) transform.localEulerAngles = new Vector3(0, 0.01f, 0);
                SetIsOpened(false);
            }
        }

    }

    private void SetIsOpened(bool isOpened)
    {
        this.isOpened = isOpened;
        openingIsHappening = false;
    }

    private static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
