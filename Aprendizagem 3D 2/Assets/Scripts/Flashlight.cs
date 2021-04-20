using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light light;
    private bool lightOn = false;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            light.enabled = !lightOn;
            lightOn = !lightOn;
        }
    }
}
