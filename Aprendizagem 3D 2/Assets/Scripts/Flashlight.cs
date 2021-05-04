using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light light;
    private bool lightOn = false;
    AudioSource audioS;
    public AudioClip lanternaON;
    public AudioClip lanternaOff;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        audioS = GetComponent<AudioSource>();
        light.enabled = false;       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            light.enabled = !lightOn;
            lightOn = !lightOn;
            if(lightOn == false){audioS.PlayOneShot(lanternaON);}
            if(lightOn == true){audioS.PlayOneShot(lanternaOff);}
           
        }
    }
}
