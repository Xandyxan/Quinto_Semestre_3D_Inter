using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float speed;
    private float actualSpeed, normalSpeed, doubleSpeed;
    public float endPoint;     //-55.87f

    private void Awake()
    {
        normalSpeed = speed;
        doubleSpeed = normalSpeed * 5f;
    }

    void Update()
    {
        if (Input.GetKey("space"))
        {
            actualSpeed = doubleSpeed;
        }
        else
        {
            actualSpeed = normalSpeed;
        }

        if (transform.position.y > endPoint) { transform.position = new Vector3(transform.position.x, transform.position.y - actualSpeed * Time.deltaTime, -10f); }
        else if (transform.position.y <= endPoint) { Invoke("GoToMenu", 2.5f); }
    }

    public void GoToMenu()
    {
        GameManager.instance.LoadScene(0);
    }
}
