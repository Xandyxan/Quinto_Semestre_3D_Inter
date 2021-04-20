using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoors : MonoBehaviour
{
    public Transform doorToRotate;
    private float velocityToRotate;
    Quaternion desireRot, normalRot;
    bool rotate, isOpen;

    // Start is called before the first frame update
    void Start()
    {
        desireRot = Quaternion.Euler(80f, transform.eulerAngles.y, transform.eulerAngles.z);
        normalRot = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        velocityToRotate = 30f;
        rotate = isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate) Rotate();
    }

    public void OpenCloseDoors(){ rotate = true; }

    private void Rotate()
    {
        if (doorToRotate.eulerAngles.x <= 0f) isOpen = rotate = false;
        else if (doorToRotate.eulerAngles.x >= desireRot.eulerAngles.x)
        {
            isOpen = true;
            rotate = false;
        }

        if (!isOpen) doorToRotate.rotation = Quaternion.RotateTowards(doorToRotate.rotation, desireRot, velocityToRotate * Time.deltaTime);
        if (isOpen) doorToRotate.rotation = Quaternion.RotateTowards(doorToRotate.rotation, normalRot, velocityToRotate * Time.deltaTime);
    }
}
