using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light light;
    private bool lightOn = false;
    [SerializeField] private string onPath, offPath;
   // [SerializeField] private Projector proj;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;
     
      //  proj.ignoreLayers = (1 << 14);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!light.isActiveAndEnabled) FMODUnity.RuntimeManager.PlayOneShot(onPath);
            else FMODUnity.RuntimeManager.PlayOneShot(offPath);

            light.enabled = !lightOn;
            lightOn = !lightOn;
        }
    }
}
